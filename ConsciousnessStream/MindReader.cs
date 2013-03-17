using ConsciousnessStream.Events;
using Emotiv;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream
{
    class MindReader
    {
        private EmoEngine engine;
        private IEventStore store;
        private StreamName streamName;
        
        public MindReader(IEventStore store, StreamName streamName,  EmoEngine engine)
        {           
            this.engine = engine;
            this.store = store;
            this.streamName = streamName;

            RegisterHandlers();
        }

        public void StartReading()
        {
            //engine.Connect();
            //engine.RemoteConnect("127.0.0.1", 3008);
            engine.RemoteConnect("127.0.0.1", 1726);

            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            
            while (true)
            {
                try
                {
                    if (Console.KeyAvailable)
                    {
                        cki = Console.ReadKey(true);
                        if (cki.Key == ConsoleKey.X)
                        {
                            break;
                        }
                    }
                    engine.ProcessEvents(1000);
                }
                catch (EmoEngineException e)
                {
                    Console.WriteLine("{0}", e.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0}", e.ToString());
                }
            }
            engine.Disconnect();
        }

        private void RegisterHandlers()
        {
            engine.AffectivEmoStateUpdated += engine_AffectivEmoStateUpdated;
            engine.CognitivAutoSamplingNeutralCompleted += engine_CognitivAutoSamplingNeutralCompleted;
            engine.CognitivEmoStateUpdated += engine_CognitivEmoStateUpdated;
            engine.CognitivSignatureUpdated += engine_CognitivSignatureUpdated;
            engine.CognitivTrainingCompleted += engine_CognitivTrainingCompleted;
            engine.CognitivTrainingDataErased += engine_CognitivTrainingDataErased;
            engine.CognitivTrainingFailed += engine_CognitivTrainingFailed;
            engine.CognitivTrainingRejected += engine_CognitivTrainingRejected;
            engine.CognitivTrainingReset += engine_CognitivTrainingReset;
            engine.CognitivTrainingStarted += engine_CognitivTrainingStarted;
            engine.CognitivTrainingSucceeded += engine_CognitivTrainingSucceeded;
            engine.EmoEngineConnected += engine_EmoEngineConnected;
            engine.EmoEngineDisconnected += engine_EmoEngineDisconnected;
            engine.ExpressivEmoStateUpdated += engine_ExpressivEmoStateUpdated;
            engine.ExpressivTrainingCompleted += engine_ExpressivTrainingCompleted;
            engine.ExpressivTrainingDataErased += engine_ExpressivTrainingDataErased;
            engine.ExpressivTrainingFailed += engine_ExpressivTrainingFailed;
            engine.ExpressivTrainingRejected += engine_ExpressivTrainingRejected;
            engine.ExpressivTrainingReset += engine_ExpressivTrainingReset;
            engine.ExpressivTrainingStarted += engine_ExpressivTrainingStarted;
            engine.ExpressivTrainingSucceeded += engine_ExpressivTrainingSucceeded;
            engine.InternalStateChanged += engine_InternalStateChanged;
            engine.UserAdded += engine_UserAdded;
            engine.UserRemoved += engine_UserRemoved;
        }

        void engine_UserRemoved(object sender, EmoEngineEventArgs e)
        {
            Store(new UserRemoved(e.userId));
        }

        void engine_UserAdded(object sender, EmoEngineEventArgs e)
        {
            Store(new UserAdded(e.userId));
        }

        void engine_InternalStateChanged(object sender, EmoEngineEventArgs e)
        {
            Store(new InternalStateChanged(e.userId));
        }

        void engine_ExpressivTrainingSucceeded(object sender, EmoEngineEventArgs e)
        {
            Store(new ExpressivTrainingSucceeded(e.userId));
        }

        void engine_ExpressivTrainingStarted(object sender, EmoEngineEventArgs e)
        {
            Store(new ExpressivTrainingStarted(e.userId));
        }

        void engine_ExpressivTrainingReset(object sender, EmoEngineEventArgs e)
        {
            Store(new ExpressivTrainingCompleted(e.userId));
        }

        void engine_ExpressivTrainingRejected(object sender, EmoEngineEventArgs e)
        {
            Store(new ExpressivTrainingRejected(e.userId));
        }

        void engine_ExpressivTrainingFailed(object sender, EmoEngineEventArgs e)
        {
            Store(new ExpressivTrainingFailed(e.userId));
        }

        void engine_ExpressivTrainingDataErased(object sender, EmoEngineEventArgs e)
        {
            Store(new ExpressivTrainingDataErased(e.userId));
        }

        void engine_ExpressivTrainingCompleted(object sender, EmoEngineEventArgs e)
        {
            Store(new ExpressivTrainingCompleted(e.userId));
        }

        void engine_ExpressivEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            
            var clenchExtent = e.emoState.ExpressivGetClenchExtent();
            var eyebrowExtent = e.emoState.ExpressivGetEyebrowExtent();
            float leftEyelid, rightEyelid;
            e.emoState.ExpressivGetEyelidState(out leftEyelid, out rightEyelid);
            float leftEyeLocation, rightEyeLocation;
            e.emoState.ExpressivGetEyeLocation(out leftEyeLocation, out rightEyeLocation);
            var lowerFaceAction = e.emoState.ExpressivGetLowerFaceAction();
            var lowerFaceActionPower = e.emoState.ExpressivGetLowerFaceActionPower();
            var smileExtent = e.emoState.ExpressivGetSmileExtent();
            var upperFaceAction = e.emoState.ExpressivGetUpperFaceAction();
            var upperFaceActionPower = e.emoState.ExpressivGetUpperFaceActionPower();
            var isBlinking = e.emoState.ExpressivIsBlink();
            var isEyesOpen = e.emoState.ExpressivIsEyesOpen();
            var isLeftWink = e.emoState.ExpressivIsLeftWink();
            var isRightWink = e.emoState.ExpressivIsRightWink();
            var isLookingDown = e.emoState.ExpressivIsLookingDown();
            var isLookingLeft = e.emoState.ExpressivIsLookingLeft();
            var isLookingRight = e.emoState.ExpressivIsLookingRight();
            var isLookingUp = e.emoState.ExpressivIsLookingUp();


            var @event = new ExpressivEmoStateUpdated(
                e.userId,
                clenchExtent,
                eyebrowExtent,
                leftEyelid,
                rightEyelid,
                leftEyeLocation,
                rightEyeLocation,
                lowerFaceAction.ToString(),
                lowerFaceActionPower,
                smileExtent,
                upperFaceAction.ToString(),
                upperFaceActionPower,
                isBlinking, isEyesOpen, isLeftWink, isRightWink, isLookingDown, isLookingLeft, isLookingRight, isLookingUp);

            Store(@event);
        }

        void engine_EmoEngineDisconnected(object sender, EmoEngineEventArgs e)
        {
            Store(new EmoEngineDisconnected(e.userId));
        }

        void engine_EmoEngineConnected(object sender, EmoEngineEventArgs e)
        {
            Store(new EmoEngineConnected(e.userId));
        }

        void engine_CognitivTrainingSucceeded(object sender, EmoEngineEventArgs e)
        {
            Store(new CognitivTrainingSucceeded(e.userId));
        }

        void engine_CognitivTrainingStarted(object sender, EmoEngineEventArgs e)
        {
            Store(new CognitivTrainingStarted(e.userId));
        }

        void engine_CognitivTrainingReset(object sender, EmoEngineEventArgs e)
        {
            Store(new CognitivTrainingRejected(e.userId));
        }

        void engine_CognitivTrainingRejected(object sender, EmoEngineEventArgs e)
        {
            Store(new CognitivTrainingRejected(e.userId));
        }

        void engine_CognitivTrainingFailed(object sender, EmoEngineEventArgs e)
        {
            Store(new CognitivTrainingReset(e.userId));
        }

        void engine_CognitivTrainingDataErased(object sender, EmoEngineEventArgs e)
        {
            Store(new CognitivTrainingDataErased(e.userId));
        }

        void engine_CognitivTrainingCompleted(object sender, EmoEngineEventArgs e)
        {
            Store(new CognitivTrainingCompleted(e.userId));
        }

        void engine_CognitivSignatureUpdated(object sender, EmoEngineEventArgs e)
        {
            Store(new CognitivSignatureUpdated(e.userId));
        }

        void engine_CognitivEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            var action = e.emoState.CognitivGetCurrentAction();
            var power = e.emoState.CognitivGetCurrentActionPower();
            var isActive = e.emoState.CognitivIsActive();

            var @event = new CognitivEmoStateUpdated(e.userId, action.ToString(), power, isActive);
            Store(@event);
        }

        void engine_CognitivAutoSamplingNeutralCompleted(object sender, EmoEngineEventArgs e)
        {
            Store(new CognitivAutoSamplingNeutralCompleted(e.userId));
        }        

        void engine_AffectivEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            var excitementShortTermScore = e.emoState.AffectivGetExcitementShortTermScore();
            var excitementLongTermScore = e.emoState.AffectivGetExcitementLongTermScore();
            var engagementBoredomScore = e.emoState.AffectivGetEngagementBoredomScore();
            var frustrationScore = e.emoState.AffectivGetFrustrationScore();
            var meditationScore = e.emoState.AffectivGetMeditationScore();

            var @event = new AffectivEmoStateUpdated(
                e.userId,
                excitementShortTermScore,
                excitementLongTermScore,
                engagementBoredomScore,
                frustrationScore,
                meditationScore);

            Store(@event);
        }

        private void Store(EmoEvent e)
        {
            this.store.Store(streamName, e);
        }
    }
}
