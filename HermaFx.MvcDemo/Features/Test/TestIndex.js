; $(document).ready(function () {
	$.fn.select2.defaults.set('theme', 'bootstrap');

	// TODO: Set language to current culture
	$('#States').select2({
		data: [ {id: 'M', text: 'Madrid'} , {id: 'B', text: 'Barcelona'} ],
		language: 'es',
		minimumInputLength: 0,
	});

	$('#Country').select2({
		data: [ { id: 'ES', text: 'España' } ],
		language: 'es',
		tags: true,
		allowClear: true,
		minimumInputLength: 0,
	});
});