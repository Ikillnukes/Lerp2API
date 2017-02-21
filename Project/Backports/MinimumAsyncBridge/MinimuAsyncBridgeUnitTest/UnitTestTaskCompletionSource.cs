﻿using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using UnityEngine.Assertions;

namespace MinimuAsyncBridgeUnitTest
{
    public class UnitTestTaskCompletionSource
    {
        
        public void TestTaskCompletionSource_Result()
        {
            GetResultShouldHaveExceptionIfTheTasksGetError();
            GetResultShouldHaveTaskCanceledExceptionIfTheTasksIsCanceled();
            GetResultShouldWaitIfTheTasksIsRunning();
        }

        private void GetResultShouldHaveExceptionIfTheTasksGetError()
        {
            var tcs = new TaskCompletionSource<int>();
            var ex = new Exception("ex1");
            tcs.SetException(ex);
            var exceptionCount = 0;

            try
            {
                var res = tcs.Task.Result;
            }
            catch (AggregateException e)
            {
                Assert.AreEqual(e.InnerExceptions.Count, 1);
                Assert.AreEqual(e.InnerExceptions.First(), ex);
                exceptionCount++;
            }
            Assert.AreEqual(exceptionCount, 1);
        }

        private void GetResultShouldHaveTaskCanceledExceptionIfTheTasksIsCanceled()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetCanceled();
            var exceptionCount = 0;

            try
            {
                var res = tcs.Task.Result;
            }
            catch (AggregateException e)
            {
                Assert.AreEqual(e.InnerExceptions.Count, 1);
                Assert.AreEqual(e.InnerExceptions.First().GetType(), typeof(TaskCanceledException));
                exceptionCount++;
            }
            Assert.AreEqual(exceptionCount, 1);
        }

        private void GetResultShouldWaitIfTheTasksIsRunning()
        {
            var tcs = new TaskCompletionSource<int>();
            Task.Delay(200).ContinueWith(_ => tcs.SetResult(321));
            var res = tcs.Task.Result;
            Assert.AreEqual(res, 321);
        }

        
        public void TestTaskCompletionSource_SetXxx()
        {
            SetResultShouldHaveExceptionIfSecondTimeCalls().Wait();
            SetCanceledShouldHaveExceptionIfSecondTimeCalls().Wait();
            SetExceptionShouldHaveExceptionIfSecondTimeCalls().Wait();
        }

        private async Task SetResultShouldHaveExceptionIfSecondTimeCalls()
        {
            var tcs = new TaskCompletionSource<int>();
            var exceptionCount = 0;
            tcs.SetResult(1);
            try
            {
                tcs.SetResult(2);
            }
            catch(Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(InvalidOperationException));
                exceptionCount++;
            }

            try
            {
                tcs.SetCanceled();
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(InvalidOperationException));
                exceptionCount++;
            }

            try
            {
                tcs.SetException(new Exception());
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(InvalidOperationException));
                exceptionCount++;
            }
            Assert.AreEqual(exceptionCount, 3);

            var r = await tcs.Task;

            Assert.AreEqual(r, 1);
        }

        private async Task SetCanceledShouldHaveExceptionIfSecondTimeCalls()
        {
            var tcs = new TaskCompletionSource<int>();
            var exceptionCount = 0;
            tcs.SetCanceled();
            try
            {
                tcs.SetResult(2);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(InvalidOperationException));
                exceptionCount++;
            }

            try
            {
                tcs.SetCanceled();
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(InvalidOperationException));
                exceptionCount++;
            }

            try
            {
                tcs.SetException(new Exception());
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(InvalidOperationException));
                exceptionCount++;
            }

            try
            {
                var r = await tcs.Task;
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(TaskCanceledException));
                exceptionCount++;
            }

            Assert.IsNull(tcs.Task.Exception);
            Assert.AreEqual(exceptionCount, 4);
        }

        private async Task SetExceptionShouldHaveExceptionIfSecondTimeCalls()
        {
            var tcs = new TaskCompletionSource<int>();
            var exceptionCount = 0;
            tcs.SetException(new Exception("first"));
            try
            {
                tcs.SetResult(2);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(InvalidOperationException));
                exceptionCount++;
            }

            try
            {
                tcs.SetCanceled();
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(InvalidOperationException));
                exceptionCount++;
            }

            try
            {
                tcs.SetException(new Exception("second"));
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(InvalidOperationException));
                exceptionCount++;
            }

            try
            {
                var r = await tcs.Task;
            }
            catch(Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(Exception));
                Assert.AreEqual(e.Message, "first");
                exceptionCount++;
            }

            Assert.IsNotNull(tcs.Task.Exception);
            Assert.AreEqual(exceptionCount, 4);
        }

        
        public void TestTaskCompletionSource_TrySetXxx()
        {
            TrySetResultShouldWorkIfFirstTime().Wait();
            TrySetCanceledShouldWorkIfFirstTime().Wait();
            TrySetExceptionShouldWorkIfFirstTime().Wait();
        }

        private async Task TrySetResultShouldWorkIfFirstTime()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.TrySetResult(1);
            tcs.TrySetResult(2);
            tcs.TrySetCanceled();
            tcs.TrySetException(new Exception());

            var res = await tcs.Task.ConfigureAwait(true);
            Assert.AreEqual(tcs.Task.Status, TaskStatus.RanToCompletion);
            Assert.AreEqual(res, 1);
        }

        private async Task TrySetCanceledShouldWorkIfFirstTime()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.TrySetCanceled();
            tcs.TrySetCanceled();
            tcs.TrySetResult(1);
            tcs.TrySetException(new Exception());

            var exceptionCount = 0;
            try
            {
                var res = await tcs.Task.ConfigureAwait(true);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(TaskCanceledException));
                exceptionCount++;
            }

            Assert.AreEqual(exceptionCount, 1);
            Assert.AreEqual(tcs.Task.Status, TaskStatus.Canceled);
        }

        private async Task TrySetExceptionShouldWorkIfFirstTime()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.TrySetException(new Exception("first"));
            tcs.TrySetException(new Exception("second"));
            tcs.TrySetResult(1);
            tcs.TrySetCanceled();

            var exceptionCount = 0;
            try
            {
                var res = await tcs.Task.ConfigureAwait(true);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "first");
                exceptionCount++;
            }

            Assert.AreEqual(exceptionCount, 1);
            Assert.AreEqual(tcs.Task.Status, TaskStatus.Faulted);
        }
    }
}
