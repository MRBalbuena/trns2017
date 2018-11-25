/// <reference path="knockout-3.4.0.debug.js" />
/// <reference path="jquery-1.10.2.js" />
/// <reference path="jquery-ui-1.11.4.js" />
/// <reference path="knockout-3.4.0.js" />

var STATE_BLOCKED_BY = 'Phrase bloked by: ';
var STATE_NO_MESSAGE = '';
var STATE_TRANSLATION_EMPTY = 'Please set a translation.';
var STATE_SAVE_ERROR = 'An error has ocurred trying to save. Please try again';

var ViewModel = function () {
    var self = this;
    self.users = ko.observableArray([]);
    self.user = ko.observable('');
    self.pwd = ko.observable('');
    self.getUsers = function () {
        return $.getJSON('api/users', function (results) {
            self.users(results);
        });
    };
    self.validateUser = function () {
        var user = self.users().find(function (u) {
            return u.name.toLowerCase() === self.user().toLowerCase() && u.pwd === self.pwd();
        });
        var valid = user ? true : false;
        if (valid) self.getTrs(true);
        self.userIsValid(valid);
    }
    self.userIsValid = ko.observable(false);
    self.getTrs = function (getDefault) {        
        //var url = (typeof rows == 'undefined') ? 'api/translation/0' : 'api/translation/' + rows;
        var url = getDefault ? 'api/translation/get' : 'api/translation/getMore';
        return $.getJSON(url, function (result) {
            self.trs(result);
            self.selectedText("");
            self.translation("");
            self.getStats();
        });        
    };
    self.getStats = function() {
        $.getJSON('api/translation/stats', function (result) {
            if (!isNaN(result.TranslationsPercent)) {
                $('.progress-bar-info').attr('style', 'width: ' + result.TranslationsPercent.toFixed(2) + '%');
                $('.progress-bar-info').attr('title', 'Phrases Translated: ' + result.Translated + ', (' + result.TranslationsPercent.toFixed(2) + '%)');
                $('.progress-bar-success').attr('style', 'width: ' + result.CheckedPercent.toFixed(2) + '%');
                $('.progress-bar-success').attr('title', 'Phrases Checked: ' + result.Checked + ', (' + result.CheckedPercent.toFixed(2) + '%)');
            }
        });
    }
    self.trs = ko.observableArray([]);
    self.selected = ko.observable(null);
    self.selectedText = ko.observable('Click a phrase');
    self.translation = ko.observable('');
    self.trsClick = function (item) {
        self.translation('');
        setMessage(STATE_NO_MESSAGE);
        self.selected(item);
        self.selectedText(item.Text);
        $.post('api/block', "=" + item.Id + "," + self.user()).done(function (result) {
            var rowStyle = "active";
            var btnStyle = "btn-default";
            $("#save").removeAttr("diabled");
            self.disabled(false);
            $("#save").removeClass("btn-success");
            $("#save").removeClass("btn-danger");
            $("table td").removeClass();
            if (result && result != self.user()) {
                rowStyle = "danger";
                btnStyle = "btn-danger";
                setMessage(STATE_BLOCKED_BY, result);
                $("#save").attr("diabled", "disabled");
                self.disabled(true);
            }
            $("table td[id=" + item.Id + "]").attr("class", rowStyle);
            $("#save").addClass(btnStyle);
        });
    };
    self.setSameText = function() {
        self.translation(self.selected().Text);
    }
    self.getMore = function () {
        self.rows = 30;
        self.getTrs(false);
    }
    self.getLess = function () {
        self.rows = 10;
        self.getTrs(true);
    }
    self.rows = 10;
    self.save = function () {
        if (self.disabled()) return;
        if (!self.translation()) {
            setMessage(STATE_TRANSLATION_EMPTY);
            $("#save").addClass("btn-danger");
            return;
        }
        self.selected().Spanish = self.translation();
        if (!self.edition()) self.selected().TransBy = self.user();
        if (self.edition()) self.selected().EditedBy = self.user();
        self.selected().CheckedBy = null;
        //console.log(self.selected());
        $.post('api/translation', self.selected()).done(function () {
            var getDefault = (self.rows === 10) ? true : false;
            self.getTrs(getDefault);
            self.edition(false);
        }).fail(function (data) {

        });
        $("#save").removeClass("btn-default").removeClass("btn-danger");
        $("#save").addClass("btn-success");
    }
    self.user = ko.observable(null);
    self.message = ko.observable('');
    self.disabled = ko.observable(false);
    self.searchResult = ko.observableArray();
    self.displaySearchResult = ko.observable(false);
    self.wordFilter = ko.observable('');
    self.getSearch = function () {
        if(self.wordFilter())        
        {
            $.getJSON('api/translation/searchByWords', 'words=' + self.wordFilter(), function (result) {
                self.searchResult(result);
                self.displaySearchResult(true);
            });
        } else {
            self.searchResult(null);
            self.displaySearchResult(false);
        }
    };
    self.check = function () {
        var data = this;
        $('#dialog-check')
        .data('data', 'data')
        .dialog({
            resizable: false,
            height: 200,
            width: 400,
            modal: true,
            buttons: {
                'Confirm': function () {
                    data.CheckedBy = self.user();
                    data.CheckedTime = new Date().getTime();
                    $.post('api/translation', data).done(function () {
                        $('#tr_' + data.Id).addClass('success');
                        self.getStats();
                    }).fail(function (data) {

                    });
                    $(this).dialog('close');
                },
                Cancel: function() {
                    $(this).dialog('close');

                }
            }
        });
    };
    self.edit = function() {
        var item = this;
        item.CheckedBy = null;
        self.selected(item);
        self.selectedText(item.Text);
        self.translation(item.Spanish);
        self.disabled(false);
        self.edition(true);
    };
    self.edition = ko.observable(false);
    self.getUnchecked = function() {
        $.getJSON('api/translation/unchecked', function (result) {
            self.searchResult(result);
            self.displaySearchResult(true);
        });
    };

    function setMessage(state, value) {
        if (state === STATE_NO_MESSAGE) self.message('');
        if (state === STATE_BLOCKED_BY) self.message(STATE_BLOCKED_BY + ' ' + value);
        if (state === STATE_TRANSLATION_EMPTY) self.message(STATE_TRANSLATION_EMPTY);
        $("table td[id=message]").attr("class", "warning");
    }

};


$(function () {
    var vm = new ViewModel();
    ko.applyBindings(vm);
    vm.getUsers();    
    $('#userName').focus();
});
