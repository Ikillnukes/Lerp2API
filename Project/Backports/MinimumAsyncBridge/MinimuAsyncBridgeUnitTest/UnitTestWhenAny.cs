﻿using System;
using System.Linq;

//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace MinimuAsyncBridgeUnitTest
{
    /// <summary>
    /// Class UnitTestWhenAny.
    /// </summary>
    public class UnitTestWhenAny
    {
        /// <summary>
        /// Tests the when any.
        /// </summary>
        public void TestWhenAny()
        {
            WhenAnyDelaysShouldAwaitForMinDelay().Wait();
            WhenAnyDelaysShouldAwaitForMinDelayWithTResult().Wait();
        }

        /// <summary>
        /// Whens any should be canceled if first item of the tasks is canceled.
        /// </summary>
        public void WhenAnyShouldBeCanceledIfFirstItemOfTheTasksIsCanceled()
        {
            WhenAnyShouldBeCanceledIfFirstItemOfTheTasksIsCancledAsync().Wait();
            WhenAnyShouldBeCanceledIfFirstItemOfTheTasksOfTResultIsCancledAsync().Wait();
        }

        /// <summary>
        /// Whens any should have exception if first item of the tasks get error.
        /// </summary>
        public void WhenAnyShouldHaveExceptionIfFirstItemOfTheTasksGetError()
        {
            WhenAnyShouldHaveExceptionIfFirstItemOfTheTasksGetErrorAsync().Wait();
            WhenAnyShouldHaveExceptionIfFirstItemOfTheTasksOfTResultGetErrorAsync().Wait();
        }

        private async Task WhenAnyShouldBeCanceledIfFirstItemOfTheTasksOfTResultIsCancledAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            Task.Delay(10).ContinueWith(_ => tcs.SetCanceled());
            var t2 = Task.Delay(500).ContinueWith(_ => 2);

            var r = await Task.WhenAny<int>(tcs.Task, t2);

            AssertExt.AreSame(r, tcs.Task);

            var exceptionCount = 0;
            try
            {
                var result = r.Result;
            }
            catch (AggregateException e)
            {
                Assert.AreEqual(1, e.InnerExceptions.Count);
                Assert.AreEqual(typeof(TaskCanceledException), e.InnerExceptions.First().GetType());
                exceptionCount++;
            }
            Assert.AreEqual(exceptionCount, 1);
        }

        private async Task WhenAnyShouldBeCanceledIfFirstItemOfTheTasksIsCancledAsync()
        {
            var tcs = new TaskCompletionSource<object>();
            Task.Delay(10).ContinueWith(_ => tcs.SetCanceled());
            var t2 = Task.Delay(500);

            var r = await Task.WhenAny(tcs.Task, t2);

            AssertExt.AreSame(r, tcs.Task);
        }

        private async Task WhenAnyShouldHaveExceptionIfFirstItemOfTheTasksOfTResultGetErrorAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            var ex = new Exception("planned exception");
            Task.Delay(10).ContinueWith(_ => tcs.SetException(ex));
            var t2 = Task.Delay(500).ContinueWith(_ => 2);

            var r = await Task.WhenAny<int>(tcs.Task, t2);

            AssertExt.AreSame(r, tcs.Task);

            var exceptionCount = 0;
            try
            {
                var result = r.Result;
            }
            catch (AggregateException e)
            {
                Assert.AreEqual(1, e.InnerExceptions.Count);
                Assert.AreEqual(ex, e.InnerExceptions.First());
                exceptionCount++;
            }
            Assert.AreEqual(exceptionCount, 1);
        }

        private async Task WhenAnyShouldHaveExceptionIfFirstItemOfTheTasksGetErrorAsync()
        {
            var tcs = new TaskCompletionSource<object>();
            var plannedException = new Exception("planned exception");
            Task.Delay(10).ContinueWith(_ => tcs.SetException(plannedException));
            var t2 = Task.Delay(500);

            var r = await Task.WhenAny(tcs.Task, t2);

            AssertExt.AreSame(r, tcs.Task);
            AssertExt.AreSame(r.Exception.InnerException, plannedException);
            Assert.AreEqual(1, r.Exception.InnerExceptions.Count);
        }

        private async Task WhenAnyDelaysShouldAwaitForMinDelayWithTResult()
        {
            var t1 = Task.Delay(10).ContinueWith(_ => 1);
            var t2 = Task.Delay(200).ContinueWith(_ => 2);
            var t3 = Task.Delay(500).ContinueWith(_ => 3);
            var t4 = Task.Delay(1000).ContinueWith(_ => 4);

            var r = await Task.WhenAny<int>(
                t2,
                t1,
                t3,
                t4).ConfigureAwait(false);

            AssertExt.AreSame(r, t1);
            Assert.AreEqual(r.Result, 1);
        }

        private async Task WhenAnyDelaysShouldAwaitForMinDelay()
        {
            var t1 = Task.Delay(10);
            var t2 = Task.Delay(200);
            var t3 = Task.Delay(500);
            var t4 = Task.Delay(1000);

            var r = await Task.WhenAny(
                t2,
                t1,
                t3,
                t4).ConfigureAwait(false);

            AssertExt.AreSame(r, t1);
        }
    }
}