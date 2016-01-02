/*
@license
dhtmlxScheduler.Net v.3.3.11 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e._temp_key_scope=function(){function t(e){delete e.rec_type,delete e.rec_pattern,delete e.event_pid,delete e.event_length}e.config.key_nav=!0;var i,n,a=null;e.attachEvent("onMouseMove",function(t,a){i=e.getActionData(a).date,n=e.getActionData(a).section}),e._make_pasted_event=function(a){var r=a.end_date-a.start_date,s=e._lame_copy({},a);if(t(s),s.start_date=new Date(i),s.end_date=new Date(s.start_date.valueOf()+r),n){var d=e._get_section_property();s[d]=e.config.multisection?a[d]:n;

}return s},e._do_paste=function(t,i,n){e.addEvent(i),e.callEvent("onEventPasted",[t,i,n])},e._is_key_nav_active=function(){return this._is_initialized()&&!this._is_lightbox_open()&&this.config.key_nav?!0:!1},dhtmlxEvent(document,_isOpera?"keypress":"keydown",function(t){if(!e._is_key_nav_active())return!0;if(t=t||event,37==t.keyCode||39==t.keyCode){t.cancelBubble=!0;var i=e.date.add(e._date,37==t.keyCode?-1:1,e._mode);return e.setCurrentView(i),!0}var n=e._select_id;if(t.ctrlKey&&67==t.keyCode)return n&&(e._buffer_id=n,
a=!0,e.callEvent("onEventCopied",[e.getEvent(n)])),!0;if(t.ctrlKey&&88==t.keyCode&&n){a=!1,e._buffer_id=n;var r=e.getEvent(n);e.updateEvent(r.id),e.callEvent("onEventCut",[r])}if(t.ctrlKey&&86==t.keyCode){var r=e.getEvent(e._buffer_id);if(r){var s=e._make_pasted_event(r);if(a)s.id=e.uid(),e._do_paste(a,s,r);else{var d=e.callEvent("onBeforeEventChanged",[s,t,!1,r]);d&&(e._do_paste(a,s,r),a=!0)}}return!0}})},e._temp_key_scope()});