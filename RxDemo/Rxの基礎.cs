using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using System.Reactive.Linq;

namespace RxDemo
{
    [TestFixture]
    public class Rxの基礎
    {
        [TestCase]
        public void まずはLinqのおさらい()
        {
            var rand = new Random();

            var DateTimeのリスト =
                (from _ in Enumerable.Range(0, 100)
                    select new DateTime(
                        (long) (rand.NextDouble()*DateTime.MaxValue.Ticks)))
                        .ToList();

            var 紀元1000年以後 = 
                from dt in DateTimeのリスト
                where dt.Year >= 1000
                select dt;

            var 閏年のみ = 
                from dt in DateTimeのリスト
                where dt.Year % 4 == 0
                select dt;

            var 隣り合う二つの差が1000年以上 =
                from dt in DateTimeのリスト
                select new {First = dt, Second = dt};

            紀元1000年以後.All(time => time.Year > 1000).IsTrue();
            閏年のみ.All(time => time.Year % 4  == 0).IsTrue();
            隣り合う二つの差が1000年以上.All(pair => Math.Abs( pair.First.Year - pair.Second.Year )> 1000).IsTrue();
        }

        [TestCase]
        public void 最初のRx()
        {
            //Rx : Reactive Extentions
            

            var Int型のObservable = Observable.Range(0, 100).Publish();

            Int型のObservable
                .Where(x => x%3 == 0)
                .Subscribe(_ => Debug.WriteLine("Fizz"));
            Int型のObservable
                .Where(x => x % 5 == 0)
                .Subscribe(_ => Debug.WriteLine("Buzz"));
            Int型のObservable
                .Where(x => x % 5 != 0 && x % 3 != 0)
                .Subscribe(num => Debug.WriteLine(num));


            Int型のObservable.Connect();

        }


    }
}
