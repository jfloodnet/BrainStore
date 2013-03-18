﻿$(function() {

    var charts = new es.Charts();

    var projection = new emoProjection({
        fromSource: fromSource,
        hub: $.connection.liveEmotivSessionHub,
        onStateUpdated: function(state) {
            charts.update(state);
        }
    });

    projection.start();
    
    function fromSource() {
        return fromCategory("EmoSession");
    }
});
