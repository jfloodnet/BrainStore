using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsciousnessStream.Events
{
    class ExpressivEmoStateUpdated : EmoEvent
    {
        public float ClenchExtent;
        public float EyebrowExtent;
        public float LeftEyelid;
        public float RightEyelid;
        public float LeftEyeLocation;
        public float RightEyeLocation;
        public string LowerFaceAction;
        public float LowerFaceActionPower;
        public float SmileExtent;
        public string UpperFaceAction;
        public float UpperFaceActionPower;
        public bool IsBlinking;
        public bool IsEyesOpen;
        public bool IsLeftWink;
        public bool IsRightWink;
        public bool IsLookingDown;
        public bool IsLookingLeft;
        public bool IsLookingRight;
        public bool IsLookingUp;

        public ExpressivEmoStateUpdated(uint userId, float clenchExtent, float eyebrowExtent, float leftEyelid, float rightEyelid, float leftEyeLocation, float rightEyeLocation, string lowerFaceAction, float lowerFaceActionPower, float smileExtent, string upperFaceAction, float upperFaceActionPower, bool isBlinking, bool isEyesOpen, bool isLeftWink, bool isRightWink, bool isLookingDown, bool isLookingLeft, bool isLookingRight, bool isLookingUp)
            :base(userId)
        {
            this.ClenchExtent = clenchExtent;
            this.EyebrowExtent = eyebrowExtent;
            this.LeftEyelid = leftEyelid;
            this.RightEyelid = rightEyelid;
            this.LeftEyeLocation = leftEyeLocation;
            this.RightEyeLocation = rightEyeLocation;
            this.LowerFaceAction = lowerFaceAction;
            this.LowerFaceActionPower = lowerFaceActionPower;
            this.SmileExtent = smileExtent;
            this.UpperFaceAction = upperFaceAction;
            this.UpperFaceActionPower = upperFaceActionPower;
            this.IsBlinking = isBlinking;
            this.IsEyesOpen = isEyesOpen;
            this.IsLeftWink = isLeftWink;
            this.IsRightWink = isRightWink;
            this.IsLookingDown = isLookingDown;
            this.IsLookingLeft = isLookingLeft;
            this.IsLookingRight = isLookingRight;
            this.IsLookingUp = isLookingUp;
        }
    }
}
