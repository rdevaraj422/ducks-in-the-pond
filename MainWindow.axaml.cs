using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Layout;
using Avalonia.Media;
using DuckSimulatorApp.Services;
using DuckSimulatorApp.Models;

namespace DuckSimulatorApp.Views;

public partial class MainWindow : Window
{
    // Swim animation timer
    private readonly DispatcherTimer _timer;

    // Random quack timer
    private readonly DispatcherTimer _quackTimer = new();
    private readonly Random _random = new();

    // Strategy Pattern: current duck object
    private Duck _currentDuck = new MallardDuck();

    // Duck visual
    private TextBlock? _duckVisual;

    // Speech bubble visuals (Border contains TextBlock)
    private Border? _speechBubbleBorder;
    private TextBlock? _speechBubbleText;

    // Duck movement
    private double _x = 30;
    private double _y = 60;
    private double _vx = 2.8;
    private double _vy = 2.2;
    private const double DuckSize = 40;

    private bool _isSwimming = true;

    public MainWindow()
    {
        InitializeComponent();

        // Hook UI events
        DuckTypeCombo.SelectionChanged += DuckTypeCombo_SelectionChanged;
        QuackBtn.Click += QuackBtn_Click;
        SwimBtn.Click += SwimBtn_Click;
        DisplayBtn.Click += DisplayBtn_Click;

        // Create visuals
        EnsureDuckVisual();
        EnsureSpeechBubble();

        // Show current duck immediately
        DuckDescriptionText.Text = _currentDuck.Description;
        StatusText.Text = $"Selected: {_currentDuck.Emoji} {_currentDuck.Name}";
        _duckVisual!.Text = _currentDuck.Emoji;

        // Start swimming automatically (~30fps)
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(33)
        };
        _timer.Tick += (_, _) => Tick();
        _timer.Start();

        // Start random quacking every 2–5 seconds
        StartRandomQuacking();
    }

    private void DuckTypeCombo_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var selected = (DuckTypeCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

        _currentDuck = selected.Contains("Mallard") ? new MallardDuck()
                    : selected.Contains("Redhead") ? new RedheadDuck()
                    : selected.Contains("Rubber") ? new RubberDuck()
                    : selected.Contains("Decoy") ? new DecoyDuck()
                    : new MallardDuck();

        DuckDescriptionText.Text = _currentDuck.Description;
        StatusText.Text = $"Selected: {_currentDuck.Emoji} {_currentDuck.Name}";

        EnsureDuckVisual();
        _duckVisual!.Text = _currentDuck.Emoji;
    }

    private void QuackBtn_Click(object? sender, RoutedEventArgs e)
    {
        var (text, soundFile) = _currentDuck.PerformQuack();

        if (!string.IsNullOrWhiteSpace(soundFile))
            AudioService.PlaySound(soundFile);

        if (!string.IsNullOrWhiteSpace(text))
        {
            StatusText.Text = $"{_currentDuck.Emoji} {text}";
            ShowSpeech(text);
        }
        else
        {
            StatusText.Text = $"{_currentDuck.Emoji} (silent...)";
        }
    }

    private void SwimBtn_Click(object? sender, RoutedEventArgs e)
    {
        _isSwimming = !_isSwimming;

        if (_isSwimming) _timer.Start();
        else _timer.Stop();

        StatusText.Text = _isSwimming ? "🌊 Swimming..." : "⏸️ Stopped swimming.";
    }

    private void DisplayBtn_Click(object? sender, RoutedEventArgs e)
    {
        EnsureDuckVisual();
        _duckVisual!.Text = _currentDuck.Emoji;

        StatusText.Text = _currentDuck.Display();
    }

    private void EnsureDuckVisual()
    {
        if (_duckVisual != null) return;

        _duckVisual = new TextBlock
        {
            Text = "🦆",
            FontSize = DuckSize,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

        PondCanvas.Children.Add(_duckVisual);
        Canvas.SetLeft(_duckVisual, _x);
        Canvas.SetTop(_duckVisual, _y);
    }

    private void EnsureSpeechBubble()
    {
        if (_speechBubbleBorder != null) return;

        _speechBubbleText = new TextBlock
        {
            FontSize = 16,
            Foreground = Brushes.Black
        };

        _speechBubbleBorder = new Border
        {
            Background = Brushes.White,
            Padding = new Thickness(8, 4),
            CornerRadius = new CornerRadius(10),
            Child = _speechBubbleText,
            IsVisible = false
        };

        PondCanvas.Children.Add(_speechBubbleBorder);
    }

    private void ShowSpeech(string text)
    {
        if (_speechBubbleBorder == null || _speechBubbleText == null) return;

        _speechBubbleText.Text = text;
        _speechBubbleBorder.IsVisible = true;

        // Position bubble slightly above duck
        Canvas.SetLeft(_speechBubbleBorder, _x + 20);
        Canvas.SetTop(_speechBubbleBorder, Math.Max(0, _y - 35));

        // Hide after 1 second
        DispatcherTimer.RunOnce(() =>
        {
            if (_speechBubbleBorder != null)
                _speechBubbleBorder.IsVisible = false;
        }, TimeSpan.FromSeconds(1));
    }

    private void Tick()
    {
        if (_duckVisual == null) return;

        var w = PondCanvas.Bounds.Width;
        var h = PondCanvas.Bounds.Height;

        if (w <= 1 || h <= 1) return;

        // Move
        _x += _vx;
        _y += _vy;

        // Bounce
        var maxX = Math.Max(0, w - DuckSize);
        var maxY = Math.Max(0, h - DuckSize);

        if (_x <= 0) { _x = 0; _vx = Math.Abs(_vx); }
        if (_x >= maxX) { _x = maxX; _vx = -Math.Abs(_vx); }

        if (_y <= 0) { _y = 0; _vy = Math.Abs(_vy); }
        if (_y >= maxY) { _y = maxY; _vy = -Math.Abs(_vy); }

        Canvas.SetLeft(_duckVisual, _x);
        Canvas.SetTop(_duckVisual, _y);

        // Keep bubble following the duck if visible
        if (_speechBubbleBorder?.IsVisible == true)
        {
            Canvas.SetLeft(_speechBubbleBorder, _x + 20);
            Canvas.SetTop(_speechBubbleBorder, Math.Max(0, _y - 35));
        }
    }

    private void StartRandomQuacking()
    {
        _quackTimer.Tick += (_, _) =>
        {
            var (text, soundFile) = _currentDuck.PerformQuack();

            if (!string.IsNullOrWhiteSpace(soundFile))
                AudioService.PlaySound(soundFile);

            if (!string.IsNullOrWhiteSpace(text))
                ShowSpeech(text);

            ScheduleNextQuack();
        };

        ScheduleNextQuack();
    }

    private void ScheduleNextQuack()
    {
        var seconds = _random.Next(2, 6); // 2–5 seconds
        _quackTimer.Interval = TimeSpan.FromSeconds(seconds);
        _quackTimer.Start();
    }
}
