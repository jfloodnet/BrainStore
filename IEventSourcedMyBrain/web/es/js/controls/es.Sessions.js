if (!window.es) { window.es = {}; };
es.EmoSessions = function (opts) {
    var sessionSelectClass = opts.sessionSelectClass;
    var onSessionSelected = opts.onSessionSelected;

    bind();
    populateEmotivSessions();

    function populateEmotivSessions() {        
        $.ajax({
            url: "es/emotivsessions/",
            type: "GET",
            success: function (data) {
                $.each(data, function () {
                    addOption(this);
                });
            }
        });        
    }

    function bind(){
    	$('.' + sessionSelectClass).change(function(){
    		var streamName = $(this).val();     		
    		onSessionSelected(streamName);
    	});    	
    }   

    function addOption(option) {
        var html = makeOptionHtml(option);
        var optgroup = findOptGroup(option.UserActivity);
        if (!found(optgroup)) {
            optgroup = makeOptGroupHtml(option.UserActivity);            
        }
        optgroup.append(html);
    }

    function makeOptionHtml(option) {
        return $("<option value='" + option.StreamId + "'>" +
                        option.DisplayName +
                 "</option>");                
    }

    function findOptGroup(optGroup) {
        return $('.' + sessionSelectClass).find("optgroup[label='" + optGroup + "']").first();
    }

    function makeOptGroupHtml(optGroup) {
        return $("<optgroup label='" + optGroup + "'></optgroup>")
                .appendTo('.' + sessionSelectClass);
    }

    function found(optgroup) {
        return optgroup.length > 0;
    }
}