using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IllusionOfDepth
{
    public class InputManager
    {

        private bool mEnterKey,
                     mUpKey,
                     mDownKey,
                     mLeftKey,
                     mRightKey,
                     mWKey,
                     mAKey,
                     mSKey,
                     mDKey,
                     mPKey,
                     mSpaceKey,
                     mEscKey,
                     mAddKey,
                     mSubtractKey;

        public bool displayControls;
        public bool displayOptions;

        public int menuOption,
                     currentMenuMax;

        KeyboardState mKeyState;

        public GameState processInputs(GameState mGameState, Player player, GameTime gameTime, Language language)
        {
            bool oldEnterKey = mEnterKey,
                 oldUpKey = mUpKey,
                 oldDownKey = mDownKey,
                 oldLeftKey = mLeftKey,
                 oldRightKey = mRightKey,
                 oldWKey = mWKey,
                 oldAKey = mAKey,
                 oldSKey = mSKey,
                 oldDKey = mDKey,
                 oldPKey = mPKey,
                 oldSpaceKey = mSpaceKey,
                 oldEscKey = mEscKey,
                 oldAddKey = mAddKey,
                 oldSubtractKey = mSubtractKey;

            mKeyState = updateKeys(Keyboard.GetState());


            //Title......................................................
            if (mGameState == GameState.Title)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds >= 3000)
                {
                    menuOption = 1;
                    currentMenuMax = 4;
                    return GameState.Menu;              //Proceed to menu
                }
                else if (mEscKey && !oldEscKey)
                {
                    return GameState.Finish;            //Quit
                }
                else
                    return GameState.Title;

            }
            //Menu.......................................................
            else if (mGameState == GameState.Menu)
            {
                if (displayControls)
                {
                    if (mEscKey && !oldEscKey)
                        displayControls = false;
                }
                else if (displayOptions)
                {
                    if (mEnterKey && !oldEnterKey)
                    {
                        if (language.mIsEnglish == true)
                            language.makeItalian();
                        else
                            language.makeEnglish();
                    }
                    else if (mEscKey && !oldEscKey)
                    {
                        displayOptions = false;
                    }
                }
                else
                {
                    if (mEnterKey && !oldEnterKey)
                    {
                        return menuInputs();                //Start game
                    }
                    else if (mDownKey && !oldDownKey)
                    {
                        if (menuOption < currentMenuMax)
                            menuOption++;
                        else
                            menuOption = 1;
                    }
                    else if (mUpKey && !oldUpKey)
                    {
                        if (menuOption > 1)
                            menuOption--;
                        else
                            menuOption = currentMenuMax;
                    }
                    else if (mEscKey && !oldEscKey)
                    {
                        return GameState.Finish;
                    }

                    //Menu controls

                }

                return GameState.Menu;

            }
            //Playing game...............................................
            else if (mGameState == GameState.Playing)
            {
                if (mPKey && !oldPKey)
                {
                    menuOption = 1;
                    currentMenuMax = 3;

                    return GameState.Paused;            //Pause game
                }



                //In-game controls
                playingInputs(player, gameTime);

                return GameState.Playing;
            }
            //Paused game................................................
            else if (mGameState == GameState.Paused)
            {
                if (displayControls)
                {
                    if (mEnterKey && !oldEnterKey)
                        displayControls = false;
                }
                else
                {

                    if (mDownKey && !oldDownKey)
                    {
                        if (menuOption < currentMenuMax)
                            menuOption++;
                        else
                            menuOption = 1;
                    }
                    else if (mUpKey && !oldUpKey)
                    {
                        if (menuOption > 1)
                            menuOption--;
                        else
                            menuOption = currentMenuMax;
                    }
                    else if (mEnterKey && !oldEnterKey)
                    {
                        //Paused controls
                        return pausedInputs();
                    }
                }


                return GameState.Paused;
            }
            else if (mGameState == GameState.Won)
            {
                if (mEnterKey && !oldEnterKey)
                {
                    return GameState.Menu;
                }
                return GameState.Won;
            }
            //Exit game..................................................
            else
            {
                return GameState.Finish;
            }


        }


        private KeyboardState updateKeys(KeyboardState keyState)
        {
            mEnterKey = keyState.IsKeyDown(Keys.Enter);
            mUpKey = keyState.IsKeyDown(Keys.Up);
            mDownKey = keyState.IsKeyDown(Keys.Down);
            mLeftKey = keyState.IsKeyDown(Keys.Left);
            mRightKey = keyState.IsKeyDown(Keys.Right);
            mWKey = keyState.IsKeyDown(Keys.W);
            mAKey = keyState.IsKeyDown(Keys.A);
            mSKey = keyState.IsKeyDown(Keys.S);
            mDKey = keyState.IsKeyDown(Keys.D);
            mPKey = keyState.IsKeyDown(Keys.P);
            mSpaceKey = keyState.IsKeyDown(Keys.Space);
            mEscKey = keyState.IsKeyDown(Keys.Escape);
            mAddKey = keyState.IsKeyDown(Keys.Add);
            mSubtractKey = keyState.IsKeyDown(Keys.Subtract);

            return keyState;
        }


        public GameState menuInputs()
        {
            if (menuOption == 1) //Play
            {
                return GameState.Playing;
            }
            else if (menuOption == 2) //Controls
            {
                displayControls = true;
                return GameState.Menu;
            }
            else if (menuOption == 3)    //Options
            {
                displayOptions = true;
                return GameState.Menu;
            }
            else //Exit
            {
                return GameState.Finish;
            }

        }

        public void playingInputs(Player player, GameTime gameTime)
        {
            player.update(mKeyState, gameTime);



        }

        public GameState pausedInputs()
        {
            if (menuOption == 1) //Resume
            {
                return GameState.Playing;
            }
            else if (menuOption == 2) //Controls
            {
                displayControls = true;
                return GameState.Paused;
            }
            else  //Return to menu
            {
                menuOption = 1;
                currentMenuMax = 4;
                return GameState.Menu;
            }

        }








    }
}
