using System;
using System.Globalization;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // 計算 BMI 的事件處理器
        private void OnCalculateClicked(object sender, EventArgs e)
        {
            ErrorLabel.IsVisible = false;
            ErrorLabel.Text = string.Empty;// 清空錯誤文字內容

            if (!double.TryParse(HeightEntry.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out var heightCm))
            { ShowError("請輸入有效的身高（公分）。"); return; }

            if (!double.TryParse(WeightEntry.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out var weightKg))
            { ShowError("請輸入有效的體重（公斤）。"); return; }

            if (heightCm < 50 || heightCm > 250) { ShowError("身高範圍應介於 50–250 公分。"); return; }
            if (weightKg < 20 || weightKg > 300) { ShowError("體重範圍應介於 20–300 公斤。"); return; }

            var bmi = weightKg / Math.Pow(heightCm / 100.0, 2); //先把公分換公尺再平方
            BmiValueLabel.Text = bmi.ToString("0.00", CultureInfo.CurrentCulture);

            var (category, color) = GetCategory(bmi);
            CategoryLabel.Text = category;
            CategoryLabel.TextColor = color;

            SemanticScreenReader.Announce($"BMI {BmiValueLabel.Text}，{category}");
        }

        //清除輸入的事件處理器
        private void OnClearClicked(object sender, EventArgs e)
        {
            HeightEntry.Text = string.Empty;
            WeightEntry.Text = string.Empty;
            ErrorLabel.IsVisible = false;
            ErrorLabel.Text = string.Empty;
            BmiValueLabel.Text = "--";
            CategoryLabel.Text = string.Empty;
        }

        //顯示錯誤訊息的方法
        private void ShowError(string message)
        {
            ErrorLabel.Text = message;
            ErrorLabel.IsVisible = true;
            BmiValueLabel.Text = "--";
            CategoryLabel.Text = string.Empty;
        }

        //BMI 分類方法
        private static (string category, Color color) GetCategory(double bmi)
        {
            if (bmi < 18.5) return ("過輕 (Underweight)", Colors.SteelBlue);
            if (bmi < 25.0) return ("正常 (Normal)", Colors.ForestGreen);
            if (bmi < 30.0) return ("過重 (Overweight)", Colors.OrangeRed);
            return ("肥胖 (Obese)", Colors.Red);
        }
    }
}
