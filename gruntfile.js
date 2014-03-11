module.exports = function (grunt) {
    require('load-grunt-tasks')(grunt);
    require('time-grunt')(grunt);
    'use strict';

    var compiledTsFilePath = 'Site/Content/admin/js/app.js';
    var slnPath = 'Meganium.sln';
    grunt.initConfig({
    
        execOpts: {
          maxBuffer: Infinity
        },

        pkg: grunt.file.readJSON('package.json'),

        clean: {
            compile: ['Site/Temp'],
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
            files: {
                src: ['Site/Content/admin/js/src/*.ts'],
                out: compiledTsFilePath,
                options: {
                    sourcemap: false
                },
            },
        },

        uglify: {
            app: {
                files: {
                    'Site/Content/admin/js/app.min.js': [compiledTsFilePath]
                }
            }
        },

        assemblyinfo: {
            options: {
                files: [
                    'Site/Site.csproj',
                    'Plugins/Plugins.csproj',
                    'Api/Api.csproj',
                    'Installer/Installer.csproj',
                    'Tests/SystemTests/SystemTests.csproj',
                    'Tests/UnitTests/UnitTests.csproj'
                ],
                info: {
                    version: "<%= pkg.version %>.0",
                    fileVersion: "<%= pkg.version %>.0",
                    company: 'Meganium',
                    product: 'Meganium Smart Site',
                    copyright: 'Copyright © Meganium ' + (new Date().getYear() + 1900)
                }
            }
        },
        
        shell:
        {
            version:
            {
                command: 'npm version patch'
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
                files: ['./Tests/UnitTests/UnitTests.csproj',
                        './Tests/SystemTests/SystemTests.csproj'],
                /*teamcity: true*/
            }
        },

        ftpush: {
            build: {
                auth: {
                    host: 'afonsof.com',
                    port: 21,
                    authKey: 'key1'
                },
                src: 'Site/Temp',
                dest: '/',
                exclusions: ['Site/**/*.cs'],
                keep: ['/Content/Uploads'],
                simple: true,
                useList: true
            }
        },

        compress: {
            main: {
                options: {
                    archive: 'pub/meganium-<%= pkg.version %>.zip',
                    level: 9
                },
                files: [
                    {
                        expand: true,
                        src: ['**/*'],
                        cwd: 'Site/temp/'
                    }
                ]
            }
        },
        
        clean: {
            postbuild: {
                src: ['Site/temp/Content/themes']
            }
        }

    });
    grunt.registerTask('ts-compile', ['ts', 'uglify']);
    grunt.registerTask('ci', ['assemblyinfo', 'msbuild', 'nunit']);
    grunt.registerTask('publish', ['ci', 'clean:postbuild', 'compress']);
    
};