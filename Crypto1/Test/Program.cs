using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CipherStuffs;
using CipherStuffs.Handshake;
using CourseWork.Benaloh.ProbabilisticSimplicityTest;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*var filePath = @"C:\Users\Ayttekao\Videos\2022-05-14 11-03-25.mkv";
            var sessionKey = Utils.GenerateIv(32);
            var loki97 = new Loki97Impl(new Encryption(), new BlockPacker(), new KeyGen(), sessionKey);
            var mode = EncryptionMode.RD;
            var algorithm = new Loki97Impl(new Encryption(), new BlockPacker(), new KeyGen(), sessionKey);
            var iv = mode is EncryptionMode.RD or EncryptionMode.RDH 
                ? Utils.GenerateIv(algorithm.GetBlockSize() * 2)
                : Utils.GenerateIv(algorithm.GetBlockSize());
            
            CipherService cipherService = new CipherService(loki97, iv);

            var file = await cipherService.Encrypt(filePath, mode);
            var aboba = cipherService.Decrypt(file, mode);
            await File.WriteAllBytesAsync(@"C:\Users\Ayttekao\Desktop\aboba.mkv", aboba);*/
            
            Console.Write("Progres...  ");

            Progress<int> progress = new Progress<int>();

            Client zxc = new Client(progress);
            
            var task = zxc.Morgernstern();            

            progress.ProgressChanged += (s, i) => { UpdateProgress(i); };

            task.Start();
            task.Wait();
        }

        public class ParallelClass
        {
            public async Task DoWork(Progress<int> progress)
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(50);
                    ((IProgress<int>)progress).Report(i);
                }
            }
        }

        public class CipSev
        {
            private ParallelClass _parallelClass = new ParallelClass();

            public async Task Abobus(Progress<int> progress)
            {
                for (var i = 0; i < 10; i++)
                {
                    await _parallelClass.DoWork(progress);
                }
            }
        }
        
        public class Client
        {
            private CipSev _cip = new CipSev();
            private Progress<int> _progress;

            public Client(Progress<int> progress)
            {
                _progress = progress;
            }

            public async Task Morgernstern()
            {
                await _cip.Abobus(_progress);
            }
        }

        public static void UpdateProgress(int iteration)
        {
            string anim = @"|/-\-";
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            Console.Write(anim[iteration % anim.Count()]);
        }

        public static Task Alg(IProgress<int> progress)
        {
            Task t = new Task
            (
                () =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(50);
                        Alg2(progress);
                        //((IProgress<int>)progress).Report(i);
                    }
                }
            );
            return t;
        }
        
        public static Task Alg2(IProgress<int> progress)
        {
            Task t = new Task
            (
                () =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(50);
                        ((IProgress<int>)progress).Report(i);
                    }
                }
            );
            return t;
        }
    }
}