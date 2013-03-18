es.Charts = function () {
    var newDataEvent = "es.newStats";
    var timeSeriesClass = "es-time-series";
    var chartTitleClass = "es-chart-title-js";
    var appendToSelector = ".wrap";
    var initialised = false;

    return {
        update: update,
        reset: reset
    };

    function update(stats) {
        if (!stats.StateEvent) {
            return;
        }
        if (initialised) {
            publishNewStat(stats);
        }
        else {
            var zoomer = prepareZoomer();
            setUpTimeSeries({ zoomer: zoomer });
            bindCharts(stats);
            initialised = true;
        }
    }
    
    function reset() {
        initialised = false;
        clearCharts();
    }
    
    function clearCharts(){
        $('.' + timeSeriesClass).remove();
    }

    function prepareZoomer() {
        var zoomer = new es.Zoomer({
            getNext: function (el) {
                return getRelativeEl(el, 1);
            },
            getPrev: function (el) {
                return getRelativeEl(el, -1);
            }
        });
        return zoomer;

        function getRelativeEl(el, offset) {
            var allElems = getAllElems();
            var index = allElems.index(el);
            if (index < 0)
                return null;
            var relative = allElems[index + offset] || null;
            return relative;
        }
    }

    function setUpTimeSeries(sets) {
        es.TimeSeries.setUp({
            updateEvent: newDataEvent,
            className: timeSeriesClass,
            titleClassName: chartTitleClass,
            appendTo: appendToSelector,
            maxLength: 50,
            zoomer: sets.zoomer
        });
    }

    function bindCharts(stats) {
        for (var statName in stats) {
            (function () {
                if (statName === "StateEvent") return;
                var currentStatName = statName; // closure
                var stat = stats[currentStatName];
                es.TimeSeries({
                    title: currentStatName,
                    getData: function (data) {
                        return data[currentStatName];
                    }
                });

            })();
        }
    }

    function publishNewStat(stat) {
        $(document).trigger(newDataEvent, [stat]);
    };


    function getAllElems() {
        // get all elements with timeseries class inside element to which they were appended
        var allElems = $(appendToSelector + " ." + timeSeriesClass);
        return allElems;
    }

    var unloading = false;  // hack around ajax errors
    $(window).bind('beforeunload', function () {
        unloading = true;
    });
}

//$(function () {

//    var newDataEvent = "es.newStats";
//    var timeSeriesClass = "es-time-series";
//    var chartTitleClass = "es-chart-title-js";
//    var appendToSelector = ".wrap";


//    buildCharts();

//    var initialised;
//    function buildCharts() {

//        es.EmoSessions({
//            sessionSelectClass: sessionSelectClass,
//            onSessionSelected: playSessionFor
//        });

//        var projection;
//        function playSessionFor (streamName){
//            if (initialised) reset();            
            
//            projection = new emoProjection({
//                streamName: streamName,
//                onStateUpdated: function (state) {
//                    success(state);
//                }
//            });

//            projection.start();
//        }

//        function reset() {
//            initialised = false;
//            projection.stop();
//            clearCharts();
//        }

//        function clearCharts(){
//            $('.' + timeSeriesClass).remove();
//        }

//        function success(stats) {
//            if(!stats.StateEvent){
//                return;
//            }
//            if (initialised) {
//                publishNewStat(stats);
//            }
//            else {
//                var zoomer = prepareZoomer({onPlay: projection.start, onStop: projection.stop});
//                setUpTimeSeries({ zoomer: zoomer });
//                bindCharts(stats);
//                initialised = true;
//            }            
//        }

//        function error(xhr, status, err) {
//            if (unloading)
//                return;
//            var msg = es.util.formatError("Couldn't build charts.", xhr);
//            $(".error").text(msg).show();
//        };

//        function prepareZoomer(sets) {
//            var zoomer = new es.Zoomer({
//                getNext: function (el) {
//                    return getRelativeEl(el, 1);
//                },
//                getPrev: function (el) {
//                    return getRelativeEl(el, -1);
//                },
//                onStop: sets.onStop,
//                onPlay: sets.onPlay
//            });
//            return zoomer;

//            function getRelativeEl(el, offset) {
//                var allElems = getAllElems();
//                var index = allElems.index(el);
//                if (index < 0)
//                    return null;
//                var relative = allElems[index + offset] || null;
//                return relative;
//            }
//        }
 
//        function setUpTimeSeries(sets) {
//            es.TimeSeries.setUp({
//                updateEvent: newDataEvent,
//                className: timeSeriesClass,
//                titleClassName: chartTitleClass,
//                appendTo: appendToSelector,
//                maxLength: 50,
//                zoomer: sets.zoomer              
//            });
//        }

//        function bindCharts(stats) {
//            for (var statName in stats) {
//                (function () {
//                    if(statName === "StateEvent") return;
//                    var currentStatName = statName; // closure
//                    var stat = stats[currentStatName];                    
//                    es.TimeSeries({
//                        title: currentStatName,
//                        getData: function (data) {
//                            return data[currentStatName];
//                        }
//                    });
                    
//                })();
//            }
//        }

//        function publishNewStat(stat) {
//            $(document).trigger(newDataEvent, [stat]);
//        };

  
//        function getAllElems() {
//            // get all elements with timeseries class inside element to which they were appended
//            var allElems = $(appendToSelector + " ." + timeSeriesClass);
//            return allElems;
//        }
//    }

//    var unloading = false;  // hack around ajax errors
//    $(window).bind('beforeunload', function () {
//        unloading = true;
//    });
//});
