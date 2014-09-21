using System;
using System.Drawing;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace RxDemoForms
{
    public partial class Drawer : Form
    {
        public Drawer()
        {
            InitializeComponent();

            RegisterRxEvents();
        }




        #region 出来上がり_イベント駆動

        ////--- ドラッグ中のフラグと前の点を覚えるプロパティ
        //private bool IsDragging { get; set; }
        //private Point PreviousLocation { get; set; }

        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    //--- 開始点を記憶
        //    this.PreviousLocation = e.Location;
        //    base.OnMouseDown(e);
        //}

        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    //--- 左ボタンが押されていればドラッグ開始
        //    if (!this.IsDragging)
        //        if (e.Button.HasFlag(MouseButtons.Left))
        //            this.IsDragging = true;

        //    //--- ドラッグ中は前の点と今の点を結ぶ
        //    if (this.IsDragging)
        //    {
        //        using (var graphic = this.CreateGraphics())
        //        using (var pen = new Pen(Color.Red, 1))
        //            graphic.DrawLine(pen, this.PreviousLocation, e.Location);
        //        this.PreviousLocation = e.Location; //--- 今の点を覚えておく
        //    }
        //    base.OnMouseMove(e);
        //}

        //protected override void OnMouseUp(MouseEventArgs e)
        //{
        //    //--- ドラッグ状態をクリア
        //    this.IsDragging = false;
        //    this.PreviousLocation = Point.Empty;
        //    base.OnMouseUp(e);
        //}

        #endregion

        #region 出来上がり_Rx

        private void RegisterRxEvents()
        {
            //--- ドラッグ処理を規定
            IObservable<MouseEventArgs> drag = this.MouseDownAsObservable()
                        .SelectMany(_ => this.MouseMoveAsObservable())
                        .TakeUntil(this.MouseUpAsObservable());
            drag.Zip //--- 前後の値でパッケージ化
            (
                drag.Skip(1),
                (x, y) => new { Prev = x.Location, Next = y.Location }
            )
            .Repeat() //--- 1ストロークで終わらず、何度も繰り返す
            .Subscribe(location =>
            {
                //--- 前の点と次の点を直線で結ぶ
                using (var graphic = this.CreateGraphics())
                using (var pen = new Pen(Color.Red, 1))
                    graphic.DrawLine(pen, location.Prev, location.Next);
            });
        }



        IObservable<MouseEventArgs> MouseDownAsObservable()
        {
            return Observable.FromEvent<MouseEventHandler, MouseEventArgs>
            (
                handler => (sender, e) => handler(e),
                handler => this.MouseDown += handler,
                handler => this.MouseDown -= handler
            );
        }

        IObservable<MouseEventArgs> MouseMoveAsObservable()
        {
            return Observable.FromEvent<MouseEventHandler, MouseEventArgs>
            (
                handler => (sender, e) => handler(e),
                handler => this.MouseMove += handler,
                handler => this.MouseMove -= handler
            );
        }

        IObservable<MouseEventArgs> MouseUpAsObservable()
        {
            return Observable.FromEvent<MouseEventHandler, MouseEventArgs>
            (
                handler => (sender, e) => handler(e),
                handler => this.MouseUp += handler,
                handler => this.MouseUp -= handler
            );
        }

        #endregion
    }
}
