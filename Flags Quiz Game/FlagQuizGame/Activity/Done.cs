using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FlagQuizGame
{
    [Activity(Label = "Done", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class Done : Activity
    {
        Button btnTryAgain;
        TextView txtTotalQuestion, txtTotalScore;
        ProgressBar progressBarResult;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Done);

            DbHelper.DbHelper db = new DbHelper.DbHelper(this);
            btnTryAgain = FindViewById<Button>(Resource.Id.btnTryAgain);
            txtTotalQuestion = FindViewById<TextView>(Resource.Id.txtTotalQuestion);
            txtTotalScore = FindViewById<TextView>(Resource.Id.txtTotalScore);
            progressBarResult = FindViewById<ProgressBar>(Resource.Id.progressBardone);
            btnTryAgain.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            };

            //Get Data 
            Bundle bundle = Intent.Extras;
            if (bundle != null)
            {
                int score = bundle.GetInt("SCORE");
                int totalQuestion = bundle.GetInt("TOTAL");
                int coreectAnswer = bundle.GetInt("CORRECT");

                //Update 2.0

                int playCount = 0;
                if (totalQuestion == 30)// Easy Mode
                {
                    playCount = db.GetPlayCount(0);
                    playCount++;
                    db.UpdatePlayCount(0, playCount);
                }
                else
                if (totalQuestion == 50)// Medium Mode
                {
                    playCount = db.GetPlayCount(1);
                    playCount++;
                    db.UpdatePlayCount(1, playCount);
                }
                else
                if (totalQuestion == 100)// Hard Mode
                {
                    playCount = db.GetPlayCount(2);
                    playCount++;
                    db.UpdatePlayCount(2, playCount);
                }
                else
                    if (totalQuestion == 200)// Hardest Mode
                {
                    playCount = db.GetPlayCount(3);
                    playCount++;
                    db.UpdatePlayCount(3, playCount);
                }

                double minus = ((5.0 / (float)score) * 100) * (playCount - 1);
                double finalScore = score - minus;
                //
                txtTotalScore.Text = $"SCORE :{ finalScore.ToString("0.00")} (-{ 5 * (playCount - 1)}%) ";
                txtTotalQuestion.Text = $"PASSED : {coreectAnswer}/{totalQuestion}";
                progressBarResult.Max = totalQuestion;
                progressBarResult.Progress = coreectAnswer;

                //Save Score
                db.InsertScore(score);
            }
        }
    }
}