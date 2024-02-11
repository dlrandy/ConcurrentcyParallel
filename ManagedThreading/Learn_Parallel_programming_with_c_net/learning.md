## Async and Await

when you await, you abandon current thread,fire up a new one and
then do a context-preserving continuation of the code that's left.

Task.WaitAll/WaitAny
Task.WhenAll/WhenAny

Task.Run equivalent to Task.Factory.StartNew(something,CancellationToken.None,
TaskCreationOptions.DenyChildAttach,
TaskScheduler.Default
) 

Task.Unwrap is used to unwrap Task<Task<int>> to Task<int>

Task.Run is a shortcut that
    1. calls task.factory.startNew
    2. Unwrap the result

await can be used as the language equivalent of Unwrap

``` c#
int result = await await Task.Factory.StartNew(
    async delegate{
        await Task.Delay(1000);
        return 42;
    },
    CancelationToken.None,
    TaskCreationOptions.DenyChildAttch,
    TaskScheduler.Default
);

```


Double await! we make a Task<Task<int>>,first await gets a Task<int>,
second await coerces it to an int;

ThreadPool.QueueUserWorkItem


Turn a LINQ query parallel by
 1. calling AsParallel on an IEnumerable
 2. Use a ParallelEnumerable

 use WithCancellation() to provide a cancellation token

 catch
  1. AggregateException
  2. OperationCanceledException if expecting to cancel

  WithMergeOptions determine how soon produced results can be consumed
  Parallel version of Aggragate provides a syntax for custom per task aggragation options

  Parallel Invoke/for/foreach



  Parallel.Xxx are blocking calls
   wait until all threads completed or an exception occured

can check the state of the loop as it it executing in ParallelLoopState.

Can check result of execution in ParallelLoopResult


ParallelLoopOptions let us custoize execution with:
 Max.degree of parallelism
 Cancellation token


 Parallel.Invoke
 runs several provided functions concurrenctly
 is equivalent to :
   Creating a task for each lambda
   Doing a Task.WaitAll() on all the tasks

Parallel.For
Uses an index [start, finish]
cannot provide a step
 Create an IEnumerable<int> and use Parallel.ForEach
 Partitions data into different tasks
 Executes provided delegate with counter value argument
  might be inefficient

Parallel.ForEach
 Liek Parallel.For() but
 Takes an IEnumerable<T> instead

 Thread Local Storage
 Writing to a shared variable from many tasks is inefficient
 can store partially evaluated results for each task
 can specify a functino to integrate partial results into final results

 Data is split into chunks by a partitioner

 can create your own
 Goal:improve performance
  E.g. void costly delegate creation calls


  ConcurrentDictionary
  Producer-consumer collections
    ConcurrentQueue
    ConcurrentStack
    ConcurrentBag

Producer-consumer pattern
  BlockingCollection



  Atomic operations:
  reference assignments;reads and writes to value types <= 32bit;
  64bit reads / writes on a 64-bit system;


















