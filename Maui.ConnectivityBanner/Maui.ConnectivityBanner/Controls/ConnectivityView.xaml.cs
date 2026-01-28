using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;

namespace Maui.ConnectivityBanner.Controls;

public partial class ConnectivityView : ContentView, IDisposable
{
    private bool _isAnimating;

    public ConnectivityView()
    {
        InitializeComponent();
        HeightRequest = 0;
        IsVisible = false;
    }

    // =======================
    // BINDABLE PROPERTIES
    // =======================

    public static readonly BindableProperty BannerHeightProperty =
        BindableProperty.Create(nameof(BannerHeight), typeof(double), typeof(ConnectivityView), 50.0);

    public double BannerHeight
    {
        get => (double)GetValue(BannerHeightProperty);
        set => SetValue(BannerHeightProperty, value);
    }

    public static readonly BindableProperty AnimationDurationProperty =
        BindableProperty.Create(nameof(AnimationDuration), typeof(int), typeof(ConnectivityView), 250);

    public int AnimationDuration
    {
        get => (int)GetValue(AnimationDurationProperty);
        set => SetValue(AnimationDurationProperty, value);
    }

    public static readonly BindableProperty HideDelayProperty =
        BindableProperty.Create(nameof(HideDelay), typeof(int), typeof(ConnectivityView), 3000);

    public int HideDelay
    {
        get => (int)GetValue(HideDelayProperty);
        set => SetValue(HideDelayProperty, value);
    }

    public static readonly BindableProperty EnableAnimationProperty =
        BindableProperty.Create(nameof(EnableAnimation), typeof(bool), typeof(ConnectivityView), true);

    public bool EnableAnimation
    {
        get => (bool)GetValue(EnableAnimationProperty);
        set => SetValue(EnableAnimationProperty, value);
    }

    public static readonly BindableProperty OfflineColorProperty =
        BindableProperty.Create(nameof(OfflineColor), typeof(Color), typeof(ConnectivityView), Colors.Red);

    public Color OfflineColor
    {
        get => (Color)GetValue(OfflineColorProperty);
        set => SetValue(OfflineColorProperty, value);
    }

    public static readonly BindableProperty OnlineColorProperty =
        BindableProperty.Create(nameof(OnlineColor), typeof(Color), typeof(ConnectivityView), Colors.Green);

    public Color OnlineColor
    {
        get => (Color)GetValue(OnlineColorProperty);
        set => SetValue(OnlineColorProperty, value);
    }

    public static readonly BindableProperty OfflineTextProperty =
        BindableProperty.Create(nameof(OfflineText), typeof(string), typeof(ConnectivityView), "Device Offline - Reconnecting...");

    public string OfflineText
    {
        get => (string)GetValue(OfflineTextProperty);
        set => SetValue(OfflineTextProperty, value);
    }

    public static readonly BindableProperty OnlineTextProperty =
        BindableProperty.Create(nameof(OnlineText), typeof(string), typeof(ConnectivityView), "Internet Restored");

    public string OnlineText
    {
        get => (string)GetValue(OnlineTextProperty);
        set => SetValue(OnlineTextProperty, value);
    }

    // =======================
    // LIFECYCLE SAFE START
    // =======================

    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (Parent != null)
        {
            Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;

            // Initial check after UI is fully rendered
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(1000); // 1 second delay for safety
                if (Parent != null)
                    await CheckStatusAsync(Connectivity.Current.NetworkAccess);
            });
        }
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        if (Handler == null)
            Dispose();
    }

    // =======================
    // CONNECTIVITY HANDLING
    // =======================

    private void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
            await CheckStatusAsync(e.NetworkAccess));
    }

    public async Task CheckStatusAsync(NetworkAccess access)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (_isAnimating) return;
            _isAnimating = true;

            if (access != NetworkAccess.Internet)
            {
                OfflineBorder.BackgroundColor = OfflineColor;
                OfflineLabel.Text = OfflineText;
                ConnectingLoader.IsVisible = true;

                IsVisible = true;
                this.AbortAnimation("BannerAnimation");

                if (!EnableAnimation)
                {
                    HeightRequest = BannerHeight;
                    _isAnimating = false;
                    return;
                }

                new Animation(v => HeightRequest = v, HeightRequest, BannerHeight)
                    .Commit(this, "BannerAnimation", 16, (uint)AnimationDuration, Easing.CubicOut,
                    (v, c) => _isAnimating = false);
            }
            else if (IsVisible && HeightRequest > 0)
            {
                OfflineBorder.BackgroundColor = OnlineColor;
                OfflineLabel.Text = OnlineText;
                ConnectingLoader.IsVisible = false;

                await Task.Delay(HideDelay);

                this.AbortAnimation("BannerAnimation");

                new Animation(v => HeightRequest = v, HeightRequest, 0)
                    .Commit(this, "BannerAnimation", 16, (uint)AnimationDuration, Easing.CubicIn,
                    (v, c) =>
                    {
                        HeightRequest = 0;
                        IsVisible = false;
                        _isAnimating = false;
                    });
            }

            _isAnimating = false;
        });
    }

    // =======================
    // CLEANUP
    // =======================

    public void Dispose()
    {
        Connectivity.Current.ConnectivityChanged -= OnConnectivityChanged;
    }
}
