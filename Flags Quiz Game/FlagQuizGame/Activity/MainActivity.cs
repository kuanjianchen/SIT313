using Android.App;
using Android.Widget;
using Android.OS;
using static Android.Widget.SeekBar;
using Android.Content;
using Android.Database.Sqlite;
using System;

namespace FlagQuizGame
{
    [Activity(Label = "FlagQuizGame", MainLauncher = true, Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : Activity, IOnSeekBarChangeListener 
    {
        SeekBar seekBar;
        TextView txtMode;
        Button btnPlay, btnScore;
        DbHelper.DbHelper db;
        SQLiteDatabase sqLiteDB;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            db = new DbHelper.DbHelper(this);
            sqLiteDB = db.WritableDatabase;

            seekBar = FindViewById<SeekBar>(Resource.Id.seekBar);
            txtMode = FindViewById<TextView>(Resource.Id.txtMode);
            btnPlay = FindViewById<Button>(Resource.Id.btnPlay);
            btnScore = FindViewById<Button>(Resource.Id.btnScore);

            seekBar.SetOnSeekBarChangeListener(this);
            btnPlay.Click += delegate
            {
                Intent intent = new Intent(this, typeof(Playing));
                intent.PutExtra("MODE", getPlayMode());
                StartActivity(intent);
                Finish();
            };
            btnScore.Click += delegate
            {
                Intent intent = new Intent(this, typeof(Score));
                StartActivity(intent);
                Finish();
            };
        }
        private String getPlayMode()
        {
            if (seekBar.Progress == 0)
                return Common.Common.MODE.EASY.ToString();
            else if (seekBar.Progress == 1)
                return Common.Common.MODE.MEDIUM.ToString();
            else if (seekBar.Progress == 2)
                return Common.Common.MODE.HARD.ToString();
            else
                return Common.Common.MODE.HARDEST.ToString();
        }
        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            txtMode.Text = getPlayMode().ToUpper();
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {

        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {

        }
    }
    }

