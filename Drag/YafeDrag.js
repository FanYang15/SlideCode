$.fn.YafeDrag = function (options) {
    //x坐标，当前标签，是否按下鼠标，是否完成校验，其他参数（后续拓展）
    var x, drag = this, isMove = false, isOver = false, defaults = {
    };

    //初始化Html标签
    drag.html('<div id="yafe_drag"><div class="yafe_bg"></div><img src="Drag/Load.jpg" class="yafe_drag_ground"  onselectstart= "return false;" unselectable= "on" /><img src="Drag/Load.jpg" class="yafe_drag_map"  onselectstart= "return false;" unselectable= "on" /><div class="yafe_drag_bg"></div><div class="yafe_drag_text yafe_slidetounlock" onselectstart= "return false;" unselectable= "on">&nbsp;</div><div class="yafe_handler yafe_handler_bg"><span></span></div><input type="hidden" name="GeelyDragID" value="" /><input type="hidden" name="GeelyDragX" value="" /></div>');

    var options = $.extend(defaults, options);//后续拓展

    var handler = drag.find('.yafe_handler');//按钮
    var drag_bg = drag.find('.yafe_drag_bg');//左边
    var text = drag.find('.yafe_drag_text');//整条
    var map = drag.find('.yafe_drag_map');//
    var ground = drag.find('.yafe_drag_ground');//背景
    var geelybg = drag.find('.yafe_bg');//背景
    var dragx = drag.find('input[name=GeelyDragX]');//当前X坐标
    var dragid = drag.find('input[name=GeelyDragID]');//当前图编号
    var maxWidth = drag.find("#yafe_drag").width() - handler.width();  //能滑动的最大间距

    init();

    //鼠标按下时
    handler.mousedown(function (e) {
        if (!isOver) {
            isMove = true;
            x = e.pageX - parseInt(handler.css('left'), 10);
            text.html("&nbsp;");
            handler.addClass('yafe_handler_run');
            drag_bg.addClass('yafe_drag_bg_run');
            dragrun();
        }
    });
    drag.find("#yafe_drag").hover(function () {
        if (!isMove) {
            ground.fadeIn(500);
            map.fadeIn(500);
            geelybg.show();
        }
    }, function () {
        if (!isMove) {
            ground.fadeOut(500);
            map.hide();
            geelybg.hide();
        }
    })


    //初始化图片
    function init() {
        $.getJSON("JsonData/GetJosn.aspx?type=get", function (result) {
            ground.attr("src", result.ground);
            map.attr("src", result.map);
            dragid.val(result.dragid);
        });
        if (handler.hasClass("yafe_handler_run"))
            handler.removeClass('yafe_handler_run').addClass('yafe_handler_error');
        if (drag_bg.hasClass("yafe_drag_bg_run"))
            drag_bg.removeClass('yafe_drag_bg_run').addClass('yafe_drag_bg_error');

        handler.animate({ 'left': 0 });
        drag_bg.animate({ 'width': 0 });
        map.animate({ 'left': 0 }, function () {
            handler.removeClass('yafe_handler_error');
            drag_bg.removeClass('yafe_drag_bg_error');
            text.html("向右滑动滑块填充拼图");
        });
    }

    function dragrun() {
        $(document).mousemove(function (e) {
            var _x = e.pageX - x;
            if (isMove) {
                if (_x > 0 && _x <= maxWidth) {
                    handler.css({ 'left': _x });
                    drag_bg.css({ 'width': _x });
                    map.css({ 'left': _x });
                }
                dragx.val(_x);
            }
        }).mouseup(function (e) {
            if (isMove) {
                isMove = false;

                $(document).unbind('mousemove');
                $(document).unbind('mouseup');

                var _x = e.pageX - x;
                dragx.val(_x);

                $.getJSON("JsonData/GetJosn.aspx?type=check&GeelyDragID=" + dragid.val() + "&GeelyDragX=" + dragx.val(), function (result) {
                    if (result.IsYes) {
                        dragOk();
                    } else {
                        init();
                    }

                });
            }
        });
    }

    //清空事件
    function dragOk() {
        isOver = true;
        handler.removeClass('yafe_handler_run').addClass('yafe_handler_ok');
        drag_bg.removeClass('yafe_drag_bg_run').addClass('yafe_drag_bg_ok');

        handler.unbind('mousedown');
        $(document).unbind('mousemove');
        $(document).unbind('mouseup');

    }

};
