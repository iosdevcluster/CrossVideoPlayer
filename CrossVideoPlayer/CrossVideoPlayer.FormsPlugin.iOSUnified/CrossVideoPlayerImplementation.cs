using CrossVideoPlayer.FormsPlugin.Abstractions;
using Xamarin.Forms;
using CrossVideoPlayer.FormsPlugin.iOSUnified;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using MediaPlayer;
using Foundation;
using AVFoundation;
using CoreMedia;
using CoreGraphics;
using AssetsLibrary;
using System;

[assembly: ExportRenderer(typeof(CrossVideoPlayerView), typeof(CrossVideoPlayerRenderer))]

namespace CrossVideoPlayer.FormsPlugin.iOSUnified
{
    /// <summary>
    /// CrossVideoPlayer Renderer for iOS (Not implemented!).
    /// </summary>
    public class CrossVideoPlayerRenderer : ViewRenderer<CrossVideoPlayerView, UIButton>
    {
        MPMoviePlayerController moviePlayer;
        private string _videoSource;
        UIButton button;
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CrossVideoPlayerView> e)
        {
            base.OnElementChanged(e);
            CrossVideoPlayerView inputView = e.NewElement ?? e.OldElement;
			_videoSource = inputView.VideoSource.Replace("/var/","/private/var/");
            
            button = new UIButton();
            ImageFor(_videoSource, 500);

            button.TouchUpInside += button_TouchUpInside;
            SetNativeControl(button);
        }

        void button_TouchUpInside(object sender, System.EventArgs e)
        {
            moviePlayer = new MPMoviePlayerController(NSUrl.FromFilename(_videoSource));
            this.NativeView.AddSubview(moviePlayer.View);
            moviePlayer.SetFullscreen(true, true);
            moviePlayer.Play();
        }

        void ImageFor(string videoPath, double time)
        {
            //if (!System.IO.File.Exists(videoPath))
            //{
            //    ALAssetsLibrary l = new ALAssetsLibrary();
            //    l.AssetForUrl(NSUrl.FromString(videoPath), (ast) =>
            //    {
            //        var cg = ast.DefaultRepresentation.GetImage();
            //        var image = new UIImage(cg);
            //        button.SetImage(image, UIControlState.Normal);
            //        button.SetBackgroundImage(image, UIControlState.Normal);
            //    }, (err) => { });
            //}
            //else
            //{
            //    var avAsset = AVAsset.FromUrl(NSUrl.FromFilename(videoPath));
            //    AVAssetImageGenerator imageGenerator = AVAssetImageGenerator.FromAsset(avAsset);
            //    imageGenerator.AppliesPreferredTrackTransform = true;

            //    CMTime actualTime;
            //    NSError error = null;
            //    var requestedTime = new CMTime((long)time, 100);
            //    using (CGImage posterImage = imageGenerator.CopyCGImageAtTime(requestedTime, out actualTime, out error))
            //    {
            //        if (posterImage != null)
            //        {
            //            var image = UIImage.FromImage(posterImage);
            //            button.SetImage(image, UIControlState.Normal);
            //            button.SetBackgroundImage(image, UIControlState.Normal);
            //        }
            //    }
            //}
            var url = NSUrl.FromFilename(videoPath);
			Console.WriteLine (" 1 " +url.AbsoluteString);
			if (!System.IO.File.Exists(videoPath))
            {
                url = NSUrl.FromString(videoPath);
				Console.WriteLine (" 2 " + url.AbsoluteString);

            }
            var avAsset = AVAsset.FromUrl(url);
            AVAssetImageGenerator imageGenerator = AVAssetImageGenerator.FromAsset(avAsset);
            imageGenerator.AppliesPreferredTrackTransform = true;

            CMTime actualTime;
            NSError error = null;
            var requestedTime = new CMTime((long)time, 100);
            using (CGImage posterImage = imageGenerator.CopyCGImageAtTime(requestedTime, out actualTime, out error))
            {
                if (posterImage != null)
                {
                    var image = UIImage.FromImage(posterImage);
                    button.SetImage(image, UIControlState.Normal);
                    button.SetBackgroundImage(image, UIControlState.Normal);
                }
            }
        }
    }
}
