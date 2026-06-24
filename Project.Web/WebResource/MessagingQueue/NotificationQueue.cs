using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using NuGet.Common;
using Project.Core.Model;
using Project.Core.NotificationProvider;
using Project.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Project.Web.WebResource.MessagingQueue
{
    public class NotificationQueue
    {
        public static int taskCount = 0;
        public static int maxThreads = 5;
        public static object lockObject = new object();
        public static bool TaskRunning = false;

        public static Queue<(int, int)> staticQueue = new Queue<(int, int)>();

        public event EventHandler Changed;
        public static Thread processingThread;

        // = new Thread(OnChanged);
        //protected virtual void OnChanged()
        //{
        //    if (Changed != null)
        //        Changed(this, EventArgs.Empty);
        //    RunQueue();
        //}

        private static void OnChanged()
        {
            processingThread = new Thread(RunQueue);
            processingThread.Start();
        }

        private static void RunQueue()
        {


            while (staticQueue.Count > 0)
            {
                var nObject = staticQueue.Dequeue();
                SaveException($"{nObject.Item1} {nObject.Item2} -current queue length{staticQueue.Count}");

                ThreadTask task = new ThreadTask(nObject);
                task.AssignNotificationTask(task);
            }
        }

        public static void AddEnqueue((int, int) item)
        {
            staticQueue.Enqueue(item);
            if (processingThread == null || !processingThread.IsAlive)
            {
                Console.WriteLine("Thread not running. Restarting...");
                OnChanged();
            }
        }

        public int Count { get { return staticQueue.Count; } }

        public virtual (int, int) Dequeue()
        {
            var item = staticQueue.Dequeue();
            return item;
        }


        private static void SaveException(string message)
        {
            ProjectDbContext ProjectDbContext = new ProjectDbContext();
            ProjectDbContext.Add(new Project.Data.DBEntities.ExceptionLogger()
            {
                Exception = message,
                CreatedOn = DateTime.Now,
                InnerException = "test",
            });
            ProjectDbContext.SaveChanges();
        }

        class ThreadTask
        {
            public int _userID;
            public int _notificationID;
            public ThreadTask((int, int) data)
            {
                (_notificationID, _userID) = data;
            }

            public void AssignNotificationTask(ThreadTask task)
            {
                Thread thread = null;

                lock (lockObject)
                {
                    // Check for thread availability
                    if (taskCount < maxThreads)
                    {
                        taskCount++;
                        System.Threading.Thread.Sleep(5000);
                        thread = new Thread(task.Execute);
                        thread.Start();
                    }
                }

                if (thread == null)
                {
                    // All threads are busy, wait for a thread to become available
                    lock (lockObject)
                    {
                        while (taskCount >= maxThreads)
                        {
                            Monitor.Wait(lockObject);
                        }

                        taskCount++;
                        System.Threading.Thread.Sleep(5000);    
                        thread = new Thread(task.Execute);
                        thread.Start();
                    }
                }
            }

            public void Execute()
            {
                // SendNotificationInBatch(_notificationModel, _data);

                lock (NotificationQueue.lockObject)
                {
                    NotificationQueue.taskCount--;
                    Monitor.PulseAll(NotificationQueue.lockObject);
                }
            }


            private void ReadNotification()
            {
                SaveException("saved");
            }
        }


    }







}
