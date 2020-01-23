(function($) {
    $.fn.draggable = function(s) {
        if (this.size() > 1) return this.each(function(i, o) { $(o).draggable(s) });
        var t = this, h = s ? t.find(s) : t, m = {}, to = false,
	      d = function(v) {
	          v.stopPropagation();
	          m = { ex: v.clientX, ey: v.clientY, x: t.css("position") == "relative" ? parseInt(t.css("left")) : t.position().left, y: t.css("position") == "relative" ? parseInt(t.css("top")) : t.position().top, fw: t.get(0).style.width, w: t.width() };
	          if (t.css("position") == "static") to = { "left": m.x, "top": m.y };
	          $(document).mousemove(b).mouseup(e);
	          if (document.body.setCapture) document.body.setCapture();
	          //debug(m)
	      },
		  b = function(v) { t.css({ "left": v.clientX - m.ex + m.x, "top": v.clientY - m.ey + m.y }); },
		  e = function(v) {
		      if (document.body.releaseCapture) document.body.releaseCapture();
		      $(document).unbind("mousemove").unbind("mouseup");
		  };
        h.mousedown(d);
        return t;
    };
})(jQuery);
