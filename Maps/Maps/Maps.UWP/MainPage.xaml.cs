using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xamarin;

namespace Maps.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            FormsMaps.Init("J6UYHMM7FiBhyBGttpnG~tB3m04Yf9qWE8r7vGzz3jg~AmKDpEn-vqbyAz7fuFowrSxWHGcXeW5ph7G2JvOcIOZyKbcSEXlFqq6IkAZQT4L2");

            LoadApplication(new Maps.App());
        }
    }
}
