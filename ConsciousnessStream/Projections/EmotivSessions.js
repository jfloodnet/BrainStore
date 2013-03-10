fromCategory('EmoSession')
.foreachStream()
.when({
    $init: function () {
        return {
            StreamId: '',
            UserActivity: '',
            SessionStarted: '',
            SessionEnded: ''
        }
    },
    'EmoEngineConnected': function (s, e) {
        var metadata = JSON.parse(e.metadataRaw);
        s.StreamId = e.streamId;
        s.UserActivity = metadata.UserActivity;
        s.SessionStarted = metadata.TimeStamp;
        return s;
    },
    'EmoEngineDisconnected': function (s, e) {
        var metadata = JSON.parse(e.metadataRaw);
        s.SessionEnded = metadata.TimeStamp;
        emit('EmoSessionSummaries', 'EmoSessionSummaryAdded', s);
    }
});
