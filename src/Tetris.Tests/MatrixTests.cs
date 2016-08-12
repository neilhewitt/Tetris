using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tetris.Core;

namespace Tetris.Tests
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void CanCreateMatrix()
        {
            Matrix matrix = null;
            Assert.DoesNotThrow(() =>
            {
                matrix = new Matrix();
            });
            Assert.That(matrix != null);
        }
    }
}
