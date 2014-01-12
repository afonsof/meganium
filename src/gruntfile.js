module.exports = function (grunt) {
    'use strict';

    // load all grunt tasks
    var compiledTsFilePath = 'Content/admin/js/app.js';
    var compiledMinFilePath = 'Content/admin/js/app.min.js';
    var slnPath = 'MegaSite.sln';
    grunt.initConfig({
    
        execOpts: {
          maxBuffer: Infinity
        },

        pkg: grunt.file.readJSON('package.json'),

        clean: {
            compile: ['MegaSite.Site/Temp'],
        },

        ts: {
            options: {
                target: 'es3',
                module: 'amd',
                sourcemap: true,
                declaration: false,
                nolib: false,
                comments: false
            },
            live: {
                src: ['MegaSite.Site/Content/admin/js/src/*.ts'],
                out: compiledTsFilePath,
                options: {
                    sourcemap: false
                },
            },
        },

        uglify: {
            my_target: {
                files: {
                    'MegaSite.Site/Content/admin/js/app.min.js': [compiledTsFilePath]
                }
            }
        },

        assemblyinfo: {
            options: {
                files: [
                    'MegaSite.Site/MegaSite.Site.csproj',
                    'MegaSite.Plugins/MegaSite.Plugins.csproj',
                    'MegaSite.Api/MegaSite.Api.csproj',
                    'MegaSite.Installer/MegaSite.Installer.csproj'
                ],
                info: {
                    version: "<%= pkg.version %>.0",
                    fileVersion: "<%= pkg.version %>.0",
                    company: 'Meganium',
                    product: 'Meganium Smart Site',
                    copyright: 'Copyright 2014 (c) Meganium',
                }
            }
        },

        msbuild: {
            src: [slnPath],
            options: {
                projectConfiguration: 'Release',
                targets: ['Clean', 'Rebuild'],
                stdout: true,
                verbosity: 'minimal',
                buildParameters: {
                    WarningLevel: 3,
                    DeployOnBuild: true,
                    PublishProfile: "temp"
                },
            }
        },

        nunit: {
            options: {
                path: 'c:\\Program Files\\NUnit\\bin',
                files: ['MegaSite.Site/MegaSite.Site.csproj']
            }
        },

        ftpush: {
            build: {
                auth: {
                    host: 'afonsof.com',
                    port: 21,
                    authKey: 'key1'
                },
                src: 'MegaSite.Site/Temp',
                dest: '/',
                exclusions: ['MegaSite.Site/**/*.cs'],
                keep: ['/Content/Uploads'],
                simple: true,
                useList: true
            }
        }

    });

    loadTasks(grunt);

    grunt.registerTask('ci', ['assemblyinfo', 'msbuild', 'nunit']);
    grunt.registerTask('default', ['ts:live']);
    grunt.registerTask('pre-build', 'Build the Typescript project', ['ts:live']);
    grunt.registerTask('post-build', 'Compiles all Typescript files to pub and minifies it', [/*'concat:dist',*/ 'uglify'/*, 'closure-compiler'*/]);
    grunt.registerTask('compile', 'Compiles all Typescript files to pub and minifies it', ['pre-build', 'post-build']);
    grunt.registerTask('compile-hint', 'Compiles, run jshint and minification', ['pre-build', 'jshint', 'post-build']);
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
    grunt.loadNpmTasks('grunt-dotnet-assembly-info'); //change assembly info
    grunt.loadNpmTasks('grunt-msbuild'); //msbuild grunt task
    grunt.loadNpmTasks('grunt-nunit-runner');
    grunt.loadNpmTasks('grunt-ftpush');
}