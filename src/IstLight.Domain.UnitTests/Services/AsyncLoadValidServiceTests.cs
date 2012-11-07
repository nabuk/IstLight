using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;
using Xunit;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using System.Threading;


namespace IstLight.UnitTests.Services
{
    public abstract class AsyncLoadValidServiceTests<T>
        where T : INamedItem
    {
        [Fact]
        public void ctor_NullService_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new AsyncLoadValidService<T>(null, x => { }));
        }

        [Fact]
        public void ctor_NullCallback_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => CreateSut(null, true));
        }

        [Fact]
        public void AddCallback_NullCallback_Throws()
        {
            var sut = CreateSut(true);
            Assert.Throws<ArgumentNullException>(() => sut.AddCallback(null));
        }

        [Fact(Timeout=5000)]
        public void AddCallbacks_BeforeLoad_WillBeExecuted()
        {
            var resume = new ManualResetEvent(false);
            var sut = CreateSut(true);
            var callback = (Action<T>)(x => resume.Set());
            sut.AddCallback(callback);
            sut.Load();
            Assert.True(resume.WaitOne());
            resume.Dispose();
        }

        [Fact]
        public void Load_CallsIAsyncLoadServiceLoadMethod()
        {
            var sutAndService = CreateSutAndServiceMock(x => { }, true);
            sutAndService.Key.Load();
            sutAndService.Value.Verify(x => x.Load(), Times.Once());
        }

        [Fact(Timeout=5000)]
        public void LoadExceptions_AreReturnedByErrorCallbackDelegate()
        {
            var resume = new ManualResetEvent(false);
            Action<Exception> errorCallback = ex => resume.Set();
            var sut = CreateSut(errorCallback, false);
            sut.Load();
            Assert.True(resume.WaitOne());
            resume.Dispose();
        }

        [Fact]
        public void Implements_ILoadValidServiceOfT()
        {
            Assert.IsAssignableFrom<IAsyncLoadValidService<T>>(CreateSut(true));
        }

        public AsyncLoadValidService<T> CreateSut(bool correct)
        {
            return CreateSut(x => { }, correct);
        }
        public AsyncLoadValidService<T> CreateSut(Action<Exception> errorCallback, bool correct)
        {
            return CreateSutAndServiceMock(errorCallback, correct).Key;
        }
        public KeyValuePair<AsyncLoadValidService<T>, Mock<IAsyncLoadService<T>>> CreateSutAndServiceMock(Action<Exception> errorCallback, bool correct)
        {
            var loadServiceMock = new Mock<IAsyncLoadService<T>>();
            loadServiceMock.Setup(x => x.Load())
                .Returns(() =>
                    new IAsyncResult<T>[]
                    { 
                        new AsyncResultFromSyncJob<T>(() =>
                            new ValueOrError<T>
                            {
                                Value = correct ? Activator.CreateInstance<T>() : default(T),
                                Error = correct ? null : new Exception()
                            })
                    }.AsReadOnlyList());
            return new KeyValuePair<AsyncLoadValidService<T>, Mock<IAsyncLoadService<T>>>(
                new AsyncLoadValidService<T>(loadServiceMock.Object, errorCallback),
                loadServiceMock);
        }
    }

    public class ClassImplementingINamedItem : INamedItem
    {
        private string name;
        public string Name
        {
            get { return name ?? (name = Guid.NewGuid().ToString()); }
        }
    }
    public struct StructImplementingINamedItem : INamedItem
    {
        private string name;
        public string Name
        {
            get { return name ?? (name = Guid.NewGuid().ToString()); }
        }
    }

    public class AsyncLoadValidServiceTests_ReferenceType : AsyncLoadValidServiceTests<ClassImplementingINamedItem> { }
    public class AsyncLoadValidServiceTests_ValueType : AsyncLoadValidServiceTests<StructImplementingINamedItem> { }
}
