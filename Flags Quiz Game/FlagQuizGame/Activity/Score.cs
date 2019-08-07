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
using FlagQuizGame.Common;

namespace FlagQuizGame
{
    [Activity(Label = "Score", Theme ="@style/AppTheme")]
    public class Score : Activity
    {
        ListView listView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Score);
            listView = FindViewById<ListView>(Resource.Id.lstView);

            DbHelper.DbHelper db = new DbHelper.DbHelper(this);
            List<Ranking> lstRanking = db.GetRanking();
            if(lstRanking.Count > 0)
            {
                CustomAdapter adapter = new CustomAdapter(this, lstRanking);
                listView.Adapter = adapter;
            }
        }
    }
}