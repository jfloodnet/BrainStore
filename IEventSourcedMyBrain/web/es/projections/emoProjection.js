
function emoProjection(options) {
    var streamName = options.streamName || "";
    var onStateUpdated = options.onStateUpdated || function () { };
    var host = options.host;

    return es.projection({        
        body: function () {
            fromStream(streamName).when({
                $init: function () {
                    var state = {};
                    state["StateEvent"] = "";
                    state["Excitement Short Term Score"] = 0;
                    state["Excitement Long Term Score"] = 0;
                    state["Frustration Score"] = 0;
                    state["Engagement Boredom Score"] = 0;
                    state["Meditation Score"] = 0;
                    state["Clench Extent"] = 0;
                    state["Eyebrow Extent"] = 0;
                    state["Left Eye Lid"] = 0;
                    state["Right Eye Lid"] = 0;
                    state["Left Eye Location"] = 0;
                    state["Right Eye Location"] = 0;
                    state["Smile Extent"] = 0;
                    return state;
                },
                AffectivEmoStateUpdated: function (state, event) {
                    var body = event.body;                    
                    state["StateEvent"] = "AffectiveEmoStateUpdated";
                    state["Excitement Short Term Score"] = body.ExcitementShortTermScore;
                    state["Excitement Long Term Score"] = body.ExcitementLongTermScore;
                    state["Frustration Score"] = body.FrustrationScore;
                    state["Engagement Boredom Score"] = body.EngagementBoredomScore;
                    state["Meditation Score"] = body.MeditationScore;
                    return state;
                },
                ExpressivEmoStateUpdated: function (state, event) {
                    var body = event.body;            
                    state["StateEvent"] = "ExpressivEmoStateUpdated";
                    state["Clench Extent"] = body.ClenchExtent;
                    state["Eyebrow Extent"] = body.EyebrowExtent;
                    state["Left Eye Lid"] = body.LeftEyelid;
                    state["Right Eye Lid"] = body.RightEyelid;
                    state["Left Eye Location"] = body.LeftEyeLocation;
                    state["Right Eye Location"] = body.RightEyeLocation;
                    state["Smile Extent"] = body.SmileExtent;
                    return state;
                }
            });
        },
        onStateUpdate: function (state) {            
            onStateUpdated(state);            
        },
        showError: function (err) {
            $('.error').text(err);
        },
        hideError: function () {
            $('.error').hide().text("");
        },
        host: host
    });
}