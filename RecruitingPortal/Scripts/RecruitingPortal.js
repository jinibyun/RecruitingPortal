var map = "";
$(document).ready(function () {

    // note: see "type" attribute below (it is not inline)
    // ref: http://stackoverflow.com/questions/15683079/fancybox-iframe-size
    $("#login").fancybox({
        type: 'iframe',//'inline',
        margin: [5, 5, 5, 5],
        padding: 10,
        width: 350,
        height: 300,
        fitToView: true,
        autoSize: false,
        //autoSize: false,
        //'transitionIn': 'elastic', // this option is for v1.3.4
        //'transitionOut': 'elastic', // this option is for v1.3.4
        //// if you want your iframe always will be 600x250 regardless the viewport size
        //'fitToView': false,  // use autoScale for v1.3.4
        //,
        //    beforeLoad: function () {
        //        // read from cookie
        //        $('#userName').val("");
        //        $("#hidPageName").val("");
        //        $("#hidParamValue").val("");

        //        $('input:checkbox[name="rememberme"]').prop('checked', false);

        //        var remember = $.cookie('pm[remember]');
        //        if (remember != "false" && remember != null) {
        //            $('#userName').val($.cookie('pm[email]'));
        //            $('input:checkbox[name="rememberme"]').prop('checked', true);
        //        }
        //    },
        //    afterClose: function () {
        //        $("#errorMessage").css("display", "none");
        //        $("#errorMessage").val("");
        //        $("#authentication-form #userName").val("");
        //        $("#authentication-form #userPwd").val("");
        //        $("#authentication-form #hidPageName").val("");
        //        $("#authentication-form #hidParamValue").val("");

        //        // $('#rememberme').attr("checked", false);
        //    }

        afterClose: function () {
            window.location.reload();
            // alert('success');
        }
    });

    // note: see "type" attribute below (it is not inline)
    // ref: http://stackoverflow.com/questions/15683079/fancybox-iframe-size
    $("#registration").fancybox({
        type: 'iframe',//'inline',
        margin: [5, 5, 5, 5],
        padding: 10,
        width: 530,
        height: 380,
        fitToView: false,
        autoSize: false
        //'transitionIn': 'elastic', // this option is for v1.3.4
        //'transitionOut': 'elastic', // this option is for v1.3.4
        // if you want your iframe always will be 600x250 regardless the viewport size
        //'fitToView': false  // use autoScale for v1.3.4
        //beforeLoad: function () {
        //    /* country select tag */
        //    // ref: http://stackoverflow.com/questions/1745704/jquery-populate-items-into-a-select-using-jquery-ajax-json-php        
        //    var selCountry = $('#selCountry').empty();
        //    selCountry.append('<option value="-1">Country</option>');
        //    SetLocation(selCountry, null, null, 0, 0, 0);
        //},
        //afterClose: function () {
        //    $("#registration-form #errorMessageReg").css("display", "none");
        //    $("#registration-form #errorMessageReg").val("");
        //    $("#registration-form #emailAccount").val("");
        //    $('#selProvince').empty();
        //    $('#selCity').empty();
        //    $('#selCountry').empty();
        //    $('input:radio[name="UserType"]').filter('[value="2"]').attr('checked', true);
        //}
    });


    

    // open sms message window per job seeker
    $('a.sendTextMessage').fancybox({
        type: 'inline',
        margin: [10, 10, 10, 10],
        padding: 20,
        width: 400,
        height: 400,//'auto',
        scrolling: 'no',
        autoSize: false,
        tpl: {
            wrap: '<div class="fancybox-wrap fancybox-login-form" tabIndex="-1"><div class="fancybox-skin"><div class="fancybox-outer"><div class="fancybox-inner"></div></div></div></div>'
        },
        openEffect: 'elastic',
        closeEffect: 'elastic',
        afterShow: function () { // pass a function, not the result of $.ajax.
            // ref: http://stackoverflow.com/questions/13060144/how-can-i-call-jquery-ajax-inside-jquery-fancybox-plugin
            // ref: http://stackoverflow.com/questions/2961496/fancybox-get-id-of-clicked-anchor-element

            var array = this.element[0].id.split("|"); // Name + "|" + CellPhone
            var jobSeekerName = array[0];
            var cellPhone = array[1];
            $("#name").text(jobSeekerName);
            $("#phone").val(cellPhone);
        },
        afterClose: function () {
            $("#name").text("");
            $("#phone").val("");
            $("#Message").val("");
        }
        
    });



    $("#btnApply").click(function () {
        var Id = $("#JobId").val();
        var url = $("#hostWebName").val() + "JobApply/Apply";

        $.ajax({
            type: "GET",
            cache: false,
            dataType: "json",
            url: url,
            data: {
                Id: Id,
                IsRedirectToAction: false
            },
            success: function (data) {
                // data is your result from controller
                if (data.success) {

                    window.location.href = $("#hostWebName").val() + "JobApply";
                }
                else {
                    alert('There was something wrong with job application');
                }
            },
            error: function (request, status, error) {
                alert(error);
            },
            complete: function () {
            }
        });
    });

    $("#btnSendSMS").click(function () {        
        // simple validation
        if ($("#Message").val() === "") {
            // fancyAlert("Please specify sms message");
            alert("Please specify sms message");
            return false;
        }

        var jobPostingId = $("#jobPostingId").text();
        var phone = $("#phone").val();
        var message = $("#Message").val();
        var url = $("#hostName").text() + "JobPosting/SendSMS";
        
        $.ajax({
            type: "GET",
            cache: false,
            dataType: "json",
            url: url,
            data: {
                phone: phone,
                message: message
            },
            success: function (data) {
                // data is your result from controller
                if (data.success) {
                    fancyAlert("Your message has been successfully sent via SMS");
                    // window.location.href = $("#hostName").val() + "JobPosting/Detail/" + jobPostingId;
                }
                else {
                    fancyAlert('There was something wrong with job application');
                }
            },
            error: function (request, status, error) {
                alert(error);
            },
            complete: function () {
            }
        });
    });



    // ref:
    // 1. http://stackoverflow.com/questions/10890948/jquery-fancybox-with-google-maps
    // 2. http://stackoverflow.com/questions/10816130/fancybox-oncomplete-function-not-running
    // 3. http://stackoverflow.com/questions/11371046/google-maps-api-v3-in-fancybox-2-0-6
    // note: refer to #1 and #3 (not #2. bug just FYI)
    $("a.mapToShowGoogle").fancybox({
        'width': 800,
        'height': 500,
        'hideOnContentClick': false, // so you can handle the map
        'overlayColor': '#ccffee',
        'overlayOpacity': 0.8,
        'autoDimensions': true, // the selector #mapcontainer HAS css width and height
        'afterShow': function () {
            var array = this.element[0].id.split("|"); // @jobPosting.city.City1|@jobPosting.city.Region.Region1|@jobPosting.PostalCode
            var city = array[0]; // city name
            var province = array[1]; // province name
            var postalCode = array[2]; // postal code

            var cityProvince = city + "," + province;

            initialize(cityProvince, postalCode);
            google.maps.event.trigger(map, "resize");
            // codeAddress("M6h 2w9");
            $("#fancybox-close").css({ "opacity": 0.5 });


        },
        'onCleanup': function () {
            var myContent = this.href;
            $(myContent).unwrap();
        } // fixes inline bug
    });
    
    //var longitude = 49.261226;
    //var latitude = -123.113928;

    //// ref: https://developers.google.com/maps/documentation/javascript/reference?hl=en#MapOptions
    //// ref: for sample for geocoding Service with javascript: https://developers.google.com/maps/documentation/javascript/examples/geocoding-simple

    //// map
    //map = new google.maps.Map(
    // document.getElementById("map_canvas"), {
    //     zoom: 9,
    //     center: new google.maps.LatLng(longitude, latitude),
    //     mapTypeId: google.maps.MapTypeId.ROADMAP
    // }
    //);

    // ref: http://stackoverflow.com/questions/12554688/how-to-dynamically-change-the-centre-of-a-google-maps-without-erasing-markers
    // ref: https://developers.google.com/maps/documentation/javascript/examples/geocoding-simple
    // note: above two concepts have been used and merged together for proper use
    function initialize(cityProvince, postalCode) {
        var mapDiv = document.getElementById('map_canvas');

        // addressParam can be eithere address or postal code
        // TODO: if case postalCode is not valid, check with cityProvince which is not being used for now.
        var address = postalCode;
        geocoder = new google.maps.Geocoder();

        // Get LatLng information by name
        geocoder.geocode({
            "address": address
        }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                map = new google.maps.Map(mapDiv, {
                    // Center map (but check status of geocoder)
                    center: results[0].geometry.location,
                    zoom: 15,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                });
                
                map.setCenter(results[0].geometry.location);
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location
                });
            } else {
                alert('Geocode was not successful for the following reason: ' + status);
            }
        });
    }

    // ref: http://jsfiddle.net/d31dwcsa/
    $(".floatNumber").on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

    // ref: http://jsfiddle.net/d31dwcsa/
    $(".onlyInteger").on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

    $('#postalCode').keyup(function () {
        // this.value = this.value.toUpperCase();
        $(this).val($(this).val().toUpperCase());
    });

    $('.PostalCode').keyup(function () {
        // this.value = this.value.toUpperCase();
        $(this).val($(this).val().toUpperCase());
    });

    // ref: http://stackoverflow.com/questions/15503839/how-to-add-a-twitter-bootstrap-tooltip-to-an-icon
    // ref: http://www.w3schools.com/bootstrap/bootstrap_tooltip.asp
    $("[rel=tooltip]").tooltip({ placement: 'right' });
    $("[data-toggle=tooltip]").tooltip({ placement: 'right' });


    // ref: http://stackoverflow.com/questions/19491336/get-url-parameter-jquery
    $.urlParam = function (name) {
        var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
        if (results == null) {
            return null;
        }
        else {
            return results[1] || 0;
        }
    }

});

// util
// note: outside of document ready. otherwise it cannot be called from other pages
function fancyAlert(msg) {
    jQuery.fancybox({
        'modal': true,
        'content': "<div style=\"margin:1px;width:300px;\"><b>" + msg + "</b><div style=\"text-align:right;margin-top:10px;\"><input class=\"btn btn-success\" type=\"button\" onclick=\"jQuery.fancybox.close();\" value=\"Ok\"></div></div>"
    });
}

function fancyAlerReload(msg) {
    jQuery.fancybox({
        'modal': true,
        'content': "<div style=\"margin:1px;width:300px;\"><b>" + msg + "</b><div style=\"text-align:right;margin-top:10px;\"><input class=\"btn btn-success\" type=\"button\" onclick=\"jQuery.fancybox.close();\" value=\"Ok\"></div></div>",
        'afterClose': function () {
            parent.location.reload(true);
        }
    });
}

// TODO: how to define below function in global level
// ref: http://stackoverflow.com/questions/10446122/javascript-jquery-callback-for-fancybox-yes-no-confirm
function fancyConfirm(msg, callbackYes, callbackNo) {
    var ret;

    jQuery.fancybox({
        'modal': true,
        'content': "<div style=\"margin:1px;width:300px;\"><b>" + msg + "</b><div style=\"text-align:right;margin-top:10px;\">" +
                   "<input id=\"fancyconfirm_cancel\"  type=\"button\" value=\"Cancel\" class=\"btn btn-warning\">&nbsp;&nbsp;&nbsp;&nbsp;" +
                   "<input id=\"fancyConfirm_ok\" type=\"button\" value=\"Ok\" class=\"btn btn-success\"></div></div>",
        'beforeShow': function () {
            jQuery("#fancyconfirm_cancel").click(function () {
                $.fancybox.close();
                callbackNo();
            });

            jQuery("#fancyConfirm_ok").click(function () {
                $.fancybox.close();
                callbackYes();
            });
        }
    });
}

