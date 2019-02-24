$(function () {
    $("div.list div").css("height", $(window).height() - 60);
    $("body").css("height", $(window).height() - 155);
    var player = document.getElementById('MuzikCalarim');
    var sarkilar = $("#listem").val();
    var sarkiListesi = sarkilar.split(',');
    var calanSarki = 0;
    var title = "ogzkyr's playlist";
    function SarkiDegistir(index) {
        var name = sarkiListesi[index].replace("/Musics/", "").replace(".mp3", "").replace(".MP3", "");
        $("div.title span").text(name.replace(" -" + name.split('-')[name.split('-').length - 1], "") + " /" + name.split('-')[name.split('-').length - 1]);
        var audioSource = player.getElementsByTagName('source');
        audioSource[0].setAttribute("src", sarkiListesi[index]);
        calanSarki = index;
        player.load();
        player.play();
        document.title = name + " | " + title;
        window.history.pushState("string", "Title", "#/" + ConvertString(name));
        $("div.list div").animate({
            scrollTop: (index - 2) * ($("div.list ul li").height() + 10)
        }, "slow");
        $("div.list ul li").removeClass("active");
        $("div.list ul li:eq(" + index + ")").addClass("active");
    }
    function ConvertString(text) {
        text = text.replace(/[. ]/g, '_');
        text = text.replace("_-_", "/");
        text = text.replace("__", "");
        return text;
    }
    function OncekiSarki() {
        SarkiDegistir(calanSarki - 1);
    }
    function SonrakiSarki() {
        SarkiDegistir(calanSarki + 1);
    }
    function RastgeleSarki() {
        var index = Math.floor(Math.random() * sarkiListesi.length);
        SarkiDegistir(index);
    }
    player.addEventListener('ended', function () {
        SonrakiSarki();
    });
    $("div.list ul li").click(function () {
        var i = $(this).attr("id");
        SarkiDegistir(i);
    });
    var ind = 0;
    var urlSplit = document.URL.split('#');
    var url = urlSplit[1];
    for (var i = 0; i < sarkiListesi.length; i++) {
        if ("/" + ConvertString(sarkiListesi[i].replace("/Musics/", "").replace(".mp3", "").replace(".MP3", "")) == url)
            ind = i;
    }
    SarkiDegistir(ind);
    $('input:radio[name="secim"]').change(function () {
        if ($(this).val() == "Sirayla") {
            player.addEventListener('ended', function () {
                SonrakiSarki();
            });
        }
        else {
            player.addEventListener('ended', function () {
                RastgeleSarki();
            });
        }
    });
    $(window).keyup(function (e) {
        if (e.keyCode == 32)
            TogglePlay();
        else if (e.keyCode == 37)
            Backward();
        else if (e.keyCode == 39)
            Forward();
        else if (e.keyCode == 38)
            VolumeUp();
        else if (e.keyCode == 40)
            VolumeDown();
        else if (e.keyCode == 77)
            ToggleMute();
    });
	var timer = 0;
    function ShowMessage(message) {
        $("div.message").text(message).stop().fadeIn(1000, function () { $("div.message").stop().fadeOut(1000); });
    }
    $("div.list").hover(function () {
        $(this).addClass("hovered");
    }, function () {
        $(this).removeClass("hovered");
    });
    function Forward() {
        if (player.duration - player.currentTime < 1)
            player.onended();
        else
            player.currentTime = player.currentTime + 10;
        ShowMessage("10sn İleri / " + parseInt(player.currentTime) / 100 + " (%" + parseInt(player.currentTime * 100 / player.duration) + ")");
    }
    function Backward() {
        if (player.currentTime < 1)
            OncekiSarki();
        else
            player.currentTime = player.currentTime - 10;
        ShowMessage("10sn Geri / " + parseInt(player.currentTime) / 100 + " (%" + parseInt(player.currentTime * 100 / player.duration) + ")");
    }
    function TogglePlay() {
        if (player.paused)
            player.play();
        else
            player.pause();
    }
    function ToggleMute() {
        player.muted = !player.muted;
        if (player.muted) {
            $("div#mute").removeClass("unmuted").addClass("muted");
            ShowMessage("Ses Kapatıldı");
        }
        else {
            $("div#mute").removeClass("muted").addClass("unmuted");
            ShowMessage("Ses Açıldı");
        }
    }
    $('body').bind('mousewheel', function (e) {
        if ($(".hovered").length == 0)
            if (e.originalEvent.wheelDelta / 120 > 0)
                VolumeUp();
            else
                VolumeDown();
    });
    player.addEventListener('timeupdate', function () {
        $("div#time").text(parseInt(player.currentTime) / 100);
        if (!$("div#slider").is(":focus"))
            $("div#slider p").css("width", parseInt(player.currentTime) / parseInt(player.duration) * 100 + "%");
    });
    player.addEventListener('durationchange', function () {
        $("div#duration").text(parseInt(player.duration) / 100);
    });
    player.addEventListener('volumechange', function () {
        $("div#volume input").val(player.volume * 10);
    });
    player.addEventListener('play', function () {
        $("div#play-pause").removeClass("paused").addClass("played");
        ShowMessage("Şarkı Çalıyor");
    });
    player.addEventListener('pause', function () {
        $("div#play-pause").removeClass("played").addClass("paused");
        ShowMessage("Şarkı Durduruldu");
    });
    function VolumeUp() {
        if (player.volume == 1)
            ShowMessage("Ses: %" + parseInt(player.volume * 100));
        else
            if (player.volume > 0.9)
                player.volume = 1;
            else
                player.volume = player.volume + 5 / 100;
        ShowMessage("Ses: %" + parseInt(player.volume * 100));
    }
    function VolumeDown() {
        if (player.volume == 0)
            ShowMessage("Ses: %" + parseInt(player.volume * 100));
        else
            if (player.volume < 0.1)
                player.volume = 0;
            else
                player.volume = player.volume - 5 / 100;
        ShowMessage("Ses: %" + parseInt(player.volume * 100));
    }

    $("div#play-pause").click(function () {
        TogglePlay();
    });
    $("div#stop").click(function () {
        player.pause();
        player.currentTime = 0;
        ShowMessage("Şarkı Durduruldu");
    });
    $("div#mute").click(function () {
        ToggleMute();
    });
    $("div#prev").click(function () {
        OncekiSarki();
    });
    $("div#random").click(function () {
        RastgeleSarki();
    });
    $("div#next").click(function () {
        SonrakiSarki();
    });
    $("div.title").click(function (e) {
        var time = parseInt(e.clientX - $(this).offset().left);
        $("div#slider p").css("width", time);
        player.currentTime = player.duration * (time / $(this).width());
    }).mousemove(function (e) {
        var time = parseInt(e.clientX - $(this).offset().left);
        $("div.title p").show().text(parseInt(player.duration * (time / $(this).width())) / 100).css({
            "left": $(window).width() - time <= $("div.title p").width() ? $(window).width() - $("div.title p").width() - 7 : time < $("div.title p").width() ? 0 : time - $("div.title p").width() / 2
        });
    }).mouseout(function () {
        $("div.title p").hide();
    });
    $("div#volume input").change(function () {
        player.volume = $(this).val() / 10;
        ShowMessage("Ses: %" + parseInt(player.volume * 100));
    });
});