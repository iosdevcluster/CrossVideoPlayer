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

[assembly: ExportRenderer(typeof(CrossVideoPlayerView), typeof(CrossVideoPlayerRenderer))]

namespace CrossVideoPlayer.FormsPlugin.iOSUnified
{
    /// <summary>
    /// CrossVideoPlayer Renderer for iOS (Not implemented!).
    /// </summary>
    public class CrossVideoPlayerRenderer : ViewRenderer<CrossVideoPlayerView, UIButton>
    {

        private string _videoSource;
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
            _videoSource = inputView.VideoSource;

            var button = new UIButton();
            var image = ImageFor(_videoSource, 2000);
            button.BackgroundColor = UIColor.FromWhiteAlpha(0.5f, 0.5f);
            button.SetImage(image, UIControlState.Normal);
            button.SetBackgroundImage(image, UIControlState.Normal);
            button.TouchUpInside += button_TouchUpInside;
            SetNativeControl(button);
        }

        void button_TouchUpInside(object sender, System.EventArgs e)
        {
            var moviePlayer = new MPMoviePlayerController(NSUrl.FromFilename(_videoSource));

            this.ViewController.View.AddSubview(moviePlayer.View);
            moviePlayer.SetFullscreen(true, true);
            moviePlayer.Play();
        }

        UIImage ImageFor(string videoPath, double time)
        {
            var avAsset = AVAsset.FromUrl(NSUrl.FromFilename(videoPath));
            AVAssetImageGenerator imageGenerator = AVAssetImageGenerator.FromAsset(avAsset);
            imageGenerator.AppliesPreferredTrackTransform = true;

            CMTime actualTime;
            NSError error = null;
            var requestedTime = new CMTime((long)time, 100);
            using (CGImage posterImage = imageGenerator.CopyCGImageAtTime(requestedTime, out actualTime, out error))
                return UIImage.FromImage(posterImage);
        }
    }
}
