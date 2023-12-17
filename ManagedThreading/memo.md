## is vs == vs equal
 1. is 不能够重写；==可以；
 2. 在进行null检查时，is忽略实际类型，统统转化为object，==则转换成同一个类型；
 3. equals 是比较内容；==是比较引用

## Thread.Sleep vs Task.Delay
Thread.Sleep是会阻塞当前线程，且不会将线程释放到线程池；task.delay还要配合await，
它会是将当前的线程释放，等到时间到，重新回到之前状态

## synchronization constructs 的类型
1. user-mode constrcuts (volatile, interlocked)
  使用特殊的CPU指令协调threads。如果一个线程不能获取资源，它将一直等待到资源可用。协调放生在硬件，所以较快。
2. kernel-mode constructs (Semaphore, mutex)
  需要OS协调。它们会使得calling thread在managed code，原生的user-mode的code和原生kernel-mode的code之间转换。
  这些上下文的切换会影响性能。
  
3. hybrid mode constructs (monitor, lock, semaphoreSlim,ReaderWriterLockSlim)
 在没有竞争时，和用户mode一样快，只有在多个线程在同一时间访问同一个资源的时候，切换成kernel-mode。

