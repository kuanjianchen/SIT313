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
using FlagQuizGame.Model;
using static Android.Views.View;

namespace FlagQuizGame
{
    [Activity(Label = "Playing", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class Playing : Activity, IOnClickListener
    {
        const long INTERVAL = 1000;
        const long TIMEOUT = 6000;
        public int progressValue = 0;

        static CountDown mCountDown;
        List<Question> questionPlay = new List<Question>();
        DbHelper.DbHelper db;
        static int index, score, thisQuestion, totalQuestion, correctAnswer;
        String mode = String.Empty;

        //Control
        public ProgressBar progressBar;
        TextView txtScore, txtQuestion;
        Button btnA, btnB, btnC, btnD;
        ImageView imageView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Playing);
            //Get data from Main Activity
            Bundle extra = Intent.Extras;
            if (extra != null)
                mode = extra.GetString("MODE");
                db = new DbHelper.DbHelper(this);
                txtScore = FindViewById<TextView>(Resource.Id.txtScore);
                txtQuestion = FindViewById<TextView>(Resource.Id.txtQuestion);
                progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
                imageView = FindViewById<ImageView>(Resource.Id.question_flag);
                btnA = FindViewById<Button>(Resource.Id.btnAnswerA);
                btnB = FindViewById<Button>(Resource.Id.btnAnswerB);
                btnC = FindViewById<Button>(Resource.Id.btnAnswerC);
                btnD = FindViewById<Button>(Resource.Id.btnAnswerD);

                btnA.SetOnClickListener(this);
                btnB.SetOnClickListener(this);
                btnC.SetOnClickListener(this);
                btnD.SetOnClickListener(this);
        }

        public void OnClick(View v)
        {
            mCountDown.Cancel();
            if (index < totalQuestion)
            {
                Button btnClicked = (Button)v;
                if (btnClicked.Text.Equals(questionPlay[index].CorrectAnswer))
                {
                    score += 10;
                    correctAnswer++;
                    ShowQuestion(++index);
                }
                else
                    ShowQuestion(++index);
            }
            txtScore.Text = $"{score}";
        }

        public void ShowQuestion(int index)
        {
            if(index < totalQuestion)
            {
                thisQuestion++;
                txtQuestion.Text = $"{thisQuestion}/{totalQuestion}";
                progressBar.Progress = progressValue = 0;
                int ImageID = this.Resources.GetIdentifier(questionPlay[index].Image.ToLower(), "drawable", PackageName);
                imageView.SetBackgroundResource(ImageID);
                btnA.Text = questionPlay[index].AnswerA;
                btnB.Text = questionPlay[index].AnswerB;
                btnC.Text = questionPlay[index].AnswerC;
                btnD.Text = questionPlay[index].AnswerD;
                mCountDown.Start();
            }
            else
            {
                Intent intent = new Intent(this, typeof(Done));
                Bundle dataSend = new Bundle();
                dataSend.PutInt("SCORE", score);
                dataSend.PutInt("TOTAL", totalQuestion);
                dataSend.PutInt("CORRECT", correctAnswer);

                intent.PutExtras(dataSend);
                StartActivity(intent);
                Finish();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            questionPlay = db.GetQuestionMode(mode);
            totalQuestion = questionPlay.Count;
            mCountDown = new CountDown(this, TIMEOUT, INTERVAL);
            ShowQuestion(index);
        }

        class CountDown : CountDownTimer
        {
            Playing playing;

            public CountDown(Playing playing, long totalTime, long intervel) : base(totalTime, intervel)
            {
                this.playing = playing;
            }

            public override void OnFinish()
            {
                Cancel();
                playing.ShowQuestion(++index);
            }

            public override void OnTick(long millisUntilFinished)
            {
                playing.progressValue++;
                playing.progressBar.Progress = playing.progressValue;
            }

        }

    }

}