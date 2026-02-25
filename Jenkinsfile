pipeline {
    agent any

    options {
        skipDefaultCheckout(false)
    }

    stages {
        stage('Checkout') {
            steps {
                deleteDir()
                checkout scm
            }
        }

        stage('Debug - Voir fichiers') {
            steps {
                bat 'dir /s'
                bat 'echo Current directory: %CD%'
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore gestion_stages.sln'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build gestion_stages.sln --configuration Release'
            }
            post {
                failure {
                    echo 'Build failed!'
                }
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test gestion_stages.sln --configuration Release --no-build'
            }
            post {
                failure {
                    echo 'Tests failed!'
                }
            }
        }
    }

    post {
        failure {
            echo 'Pipeline failed!'
        }
        success {
            echo 'Pipeline succeeded!'
        }
    }
}
