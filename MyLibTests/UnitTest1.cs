using System.Threading.Tasks;
using MyLib;
using Xunit;

namespace MyLibTests
{
    public class UnitTest1
    {
        private readonly Class1 _myLib;

        public UnitTest1()
        {
            _myLib = new Class1();
        }

        [Fact]
        public async Task Test1()
        {
            await _myLib.Run(default);
        }
    }
}
