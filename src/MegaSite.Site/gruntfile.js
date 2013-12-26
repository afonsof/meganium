module.exports = function (grunt) {
    'use strict';

    // load all grunt tasks
    var compiledTsFilePath = 'Content/admin/js/app.js';
    var compiledMinFilePath = 'Content/admin/js/app.min.js';
    grunt.initConfig({
        clean: {
            compile: ['Scripts'],
        },

        ts: {
            options: {
                // use to override the default options, See : http://gruntjs.com/configuring-tasks#options
                target: 'es3',            // es3 (default) / or es5
                module: 'amd',       // amd (default), commonjs
                sourcemap: true,          // true  (default) | false
                declaration: false,       // true | false  (default)
                nolib: false,             // true | false (default)
                comments: false           // true | false (default)
            },
            live: {
                // a particular target
                src: ['Content/admin/js/src/*.ts'], // The source typescript files, See : http://gruntjs.com/configuring-tasks#files
                //watch: 'ts',         // If specified, configures this target to watch the specified director for ts changes and reruns itself.
                out: compiledTsFilePath,
                options: {
                    // override the main options, See : http://gruntjs.com/configuring-tasks#options
                    sourcemap: false
                },
            },
        },

        uglify: {
            my_target: {
                files: {
                    'Content/admin/js/app.min.js': [compiledTsFilePath]
                }
            }
        },
    });
    
    loadTasks(grunt);

    grunt.registerTask('default', ['ts:live']);
    grunt.registerTask('pre-build', 'Build the Typescript project', ['clean', 'ts:live']);
    grunt.registerTask('post-build', 'Compiles all Typescript files to pub and minifies it', [/*'concat:dist',*/ 'uglify'/*, 'closure-compiler'*/]);
    grunt.registerTask('compile', 'Compiles all Typescript files to pub and minifies it', ['pre-build', 'post-build']);
    grunt.registerTask('compile-hint', 'Compiles, run jshint and minification', ['pre-build','jshint', 'post-build']);
    grunt.registerTask('pre-commit', 'Compiles the Typescript files, minify and run the unit tests', ['pre-build', 'post-build', 'qunit']);
};


function loadTasks(grunt) {
    grunt.loadNpmTasks('grunt-ts'); //compiles typescript
    grunt.loadNpmTasks('grunt-contrib-uglify'); // minifies
    grunt.loadNpmTasks('grunt-contrib-copy'); // copy files
    grunt.loadNpmTasks('grunt-contrib-clean'); // erase files and folders
    grunt.loadNpmTasks('grunt-contrib-concat'); // concat files
    grunt.loadNpmTasks('grunt-contrib-jshint'); // lint js files
    grunt.loadNpmTasks('grunt-contrib-qunit'); // unit tests support
    grunt.loadNpmTasks('grunt-closure-compiler'); //closure compiler
}