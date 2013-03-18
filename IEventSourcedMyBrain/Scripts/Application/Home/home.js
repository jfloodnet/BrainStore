$(function() {
    var sessionSelectClass = "emo-sessions";
    es.EmoSessions({
        sessionSelectClass: sessionSelectClass,
        onSessionSelected: playSessionFor
    });

    var charts = new es.Charts();
    
    function playSessionFor(streamName) {

        charts.reset();
        
        var projection = new emoProjection({
            fromSource: fromSource,
            hub: $.connection.historicalEmotivSessionHub,
            onStateUpdated: function(state) {
                charts.update(state);
            }
        });

        projection.start();
        
        function fromSource() {
            return fromStream(streamName);
        }
    }
});