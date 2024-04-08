using ArchiveUtil.Enums;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ArchiveUtil
{
    /// <summary>
    /// Queue for slices of file data processing.
    /// Supports sharing access between multiple threads.
    /// </summary>
    public class FileSliceQueue
    {
        private readonly FileQueueMode _queueMode;
        private readonly int _threadNumber;

        private Queue<FileSlice> _queue = new Queue<FileSlice>();
        private int _addedSlicesNumber = 0;
        private bool _stopped = false;

        public FileSliceQueue(FileQueueMode queueMode, ref int threadNumber)
        {
            _queueMode = queueMode;
            _threadNumber = threadNumber;
        }

        public Queue<FileSlice> Queue
        {
            get { return _queue; }
        }

        /// <summary>
        /// Number of slices added to the queue.
        /// </summary>
        public int AddedSlicesNumber
        {
            get { return _addedSlicesNumber; }
        }

        public bool Stopped
        {
            get { return _stopped; }
        }

        /// <summary>
        /// Stops the queue work. Threads waiting to access the queue will exit.
        /// </summary>
        public void Stop()
        {
            lock (_queue)
            {
                _stopped = true;
                // Оповещаем ожидающие потоки о завершении работы.
                Monitor.PulseAll(_queue);
            }
        }

        /// <summary>
        /// Adding slice to the queue.
        /// </summary>
        /// <returns> true - success, otherwise - false.</returns>
        public virtual bool Add(FileSlice slice)
        {
            if (slice == null)
            {
                return false;
            }

            while (_queueMode.Equals(FileQueueMode.Process) && !СheckAvailableMemory())
            {
                //Console.WriteLine($"{Thread.CurrentThread.Name} waiting out of memory.");
                Thread.Sleep(500);
            }

            lock (_queue)
            {
                while (!CheckTurn(slice))
                {
                    //Console.WriteLine($"{Thread.CurrentThread.Name} waiting not my turn.");
                    Monitor.Wait(_queue);
                }
                if (_stopped)
                {
                    return false;
                }
                _queue.Enqueue(slice);
                _addedSlicesNumber++;
                Monitor.PulseAll(_queue);
            }

            return true;
        }

        /// <summary>
        /// Retrieving the first slice from the queue.
        /// </summary>
        /// <returns>byte[] - success, otherwise - null.</returns>
        public FileSlice Take()
        {
            if (_queue.Count == 0 && _stopped)
            {
                return null;
            }
            FileSlice element;
            lock (_queue)
            {
                while (_queue.Count == 0)
                {
                    if (_stopped)
                    {
                        return null;
                    }
                    //Console.WriteLine($"{Thread.CurrentThread.Name} waiting queue is empty.");
                    Monitor.Wait(_queue);
                }

                element = _queue.Dequeue();
                Monitor.PulseAll(_queue);
            }
            return element;
        }

        /// <summary>
        /// Checks for free RAM.
        /// </summary>
        /// <returns>true - there is free memory, otherwise - false.</returns>
        private bool СheckAvailableMemory()
        {
            // The amount of available RAM must be more than 500 MB.
            return new ComputerInfo().AvailablePhysicalMemory > 500 * 1024 * 1024;
        }

        /// <summary>
        /// Checks is free space in the queue and the order in which it is added to the queue.
        /// </summary>
        /// <param name="element">Элемент, который хотим записать в очередь. Передается для проверки его порядкового номера. </param>
        /// <returns>true - место в очереди есть и подошла очередь на добавление элемента, 
        /// false - места в очереди нет или еще не подошла очередь на добавление элемента. </returns>
        private bool CheckTurn(FileSlice slice)
        {
            // Checking, is turn of slice to be added to the queue.
            var isSliceTurn = slice.Index == AddedSlicesNumber;

            // If queue is for compression and the number of slices in the queue is less than the limit equal to the number of threads performing encoding/decoding.
            // If queue is for writing and the number of slices in the queue is less than the limit equal to the number of threads performing encoding/decoding.
            var hasFreeSpace = Queue.Count < (_queueMode.Equals(FileQueueMode.Process) ? _threadNumber - 2 : _threadNumber - 2);

            return isSliceTurn && hasFreeSpace;
        }
    }
}
