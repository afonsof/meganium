using MegaSite.Api;
using MegaSite.Api.Managers;
using NUnit.Framework;

namespace MegaSite.Tests.Api
{
    /// <summary>
    /// Summary description for FileSaverTest
    /// </summary>
    [TestFixture]
    public class FileSaverTest
    {
        [Test]
        public void ResizeCropWgH()
        {
            var result = MediaFileManager.GetResizeData(400, 200, 300, 100, true);
            
            Assert.AreEqual(300, result.Item1.Width);
            Assert.AreEqual(150, result.Item1.Height);
            Assert.AreEqual(300, result.Item2.Width);
            Assert.AreEqual(100, result.Item2.Height);
        }

        [Test]
        public void ResizeCropHgW()
        {
            var result = MediaFileManager.GetResizeData(200, 400, 100, 50, true);

            Assert.AreEqual(100, result.Item1.Width);
            Assert.AreEqual(200, result.Item1.Height);
            Assert.AreEqual(100, result.Item2.Width);
            Assert.AreEqual(50, result.Item2.Height);
        }

        [Test]
        public void ResizeCropHZero()
        {
            var result = MediaFileManager.GetResizeData(200, 400, 100, 0, true);

            Assert.AreEqual(100, result.Item1.Width);
            Assert.AreEqual(200, result.Item1.Height);
            Assert.AreEqual(100, result.Item2.Width);
            Assert.AreEqual(200, result.Item2.Height);
        }

        [Test]
        public void ResizeCropEnlarge()
        {
            var result = MediaFileManager.GetResizeData(200, 400, 500, 1000, true);

            Assert.AreEqual(200, result.Item1.Width);
            Assert.AreEqual(400, result.Item1.Height);
            Assert.AreEqual(200, result.Item2.Width);
            Assert.AreEqual(400, result.Item2.Height);
        }

        [Test]
        public void ResizeCropGt3000()
        {
            var result = MediaFileManager.GetResizeData(5000, 5000, 3001, 3001, true);

            Assert.AreEqual(3000, result.Item1.Width);
            Assert.AreEqual(3000, result.Item1.Height);
            Assert.AreEqual(3000, result.Item2.Width);
            Assert.AreEqual(3000, result.Item2.Height);
        }

        [Test]
        public void ResizeCropWZero()
        {
            var result = MediaFileManager.GetResizeData(200, 400, 0, 100, true);

            Assert.AreEqual(50, result.Item1.Width);
            Assert.AreEqual(100, result.Item1.Height);
            Assert.AreEqual(50, result.Item2.Width);
            Assert.AreEqual(100, result.Item2.Height);
        }

        [Test]
        public void ResizeWgH()
        {
            var result = MediaFileManager.GetResizeData(400, 200, 300, 300, false);

            Assert.AreEqual(300, result.Item1.Width);
            Assert.AreEqual(150, result.Item1.Height);
            Assert.AreEqual(300, result.Item2.Width);
            Assert.AreEqual(150, result.Item2.Height);
        }

        [Test]
        public void ResizeHgW()
        {
            var result = MediaFileManager.GetResizeData(200, 400, 300, 300, false);

            Assert.AreEqual(150, result.Item1.Width);
            Assert.AreEqual(300, result.Item1.Height);
            Assert.AreEqual(150, result.Item2.Width);
            Assert.AreEqual(300, result.Item2.Height);
        }

        [Test]
        public void ResizeHZero()
        {
            var result = MediaFileManager.GetResizeData(200, 400, 300, 0, false);

            Assert.AreEqual(300, result.Item1.Width);
            Assert.AreEqual(600, result.Item1.Height);
            Assert.AreEqual(300, result.Item2.Width);
            Assert.AreEqual(600, result.Item2.Height);
        }

        [Test]
        public void ResizeWZero()
        {
            var result = MediaFileManager.GetResizeData(200, 400, 0, 100, false);

            Assert.AreEqual(50, result.Item1.Width);
            Assert.AreEqual(100, result.Item1.Height);
            Assert.AreEqual(50, result.Item2.Width);
            Assert.AreEqual(100, result.Item2.Height);
        }
    }
}
