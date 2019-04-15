using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IllusionOfDepth
{
    public class Language
    {
        //1       DuckShot;
        //2       Return;
        //3       Menu;
        //4       Play;
        //5       Controls;
        //6       Movement;
        //7       Arrows;
        //8       Pause;
        //9       P;
        //10      Quit;
        //11      Esc;
        //12      Credits;
        //13      Made_by;
        //14      Spencer_Martin;
        //15      word_Language;
        //16      English;
        //17      Italian;
        //18      Exit;
        //19      Paused;

        public string ControlsString;
        public string LanguageString;
        public string CreditString;
        public bool mIsEnglish;
        string[] mLines;

        public Language()
        {
            makeEnglish();
        }

        public void makeEnglish()
        {
            mLines = File.ReadAllLines("English.txt");
            Console.WriteLine(mLines[0] + mLines[1]);

            mIsEnglish = true;

            setControlsString();
            setLanguageString();
            setCreditString();
        }

        public void makeItalian()
        {
            mLines = File.ReadAllLines("Italian.txt");

            mIsEnglish = false;

            setControlsString();
            setLanguageString();
            setCreditString();
        }

        public void setControlsString()
        {
            ControlsString = mLines[5] + "\n" + mLines[6] + "\n\n" + mLines[7] + "\n" + mLines[8];
        }

        public void setCreditString()
        {
            CreditString = mLines[12] + " " + mLines[13];
        }


        public void setLanguageString()
        {
            if (mIsEnglish)
                LanguageString = mLines[14] + " : " + mLines[15];
            else
                LanguageString = mLines[14] + " : " + mLines[16];

        }

        public string[] getLines()
        {
            return mLines;
        }


    }
}
