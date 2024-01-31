pipeline {
    agent any

    environment {
        DOCKER_REGISTRY = 'your-docker-registry'
        IMAGE_NAME = 'your-image-name'
        IMAGE_TAG = 'latest'
        CONTAINER_NAME = 'your-container-name'
    }

    stages {
        stage('Checkout') {
            steps {
                // Checkout your source code from version control
                git 'https://your.git.repo/url.git'
            }
        }

        stage('Build') {
            steps {
                // Build the .NET Core project
                script {
                    sh 'dotnet build -c Release'
                }
            }
        }

        stage('Test') {
            steps {
                // Run tests (optional, based on your project)
                script {
                    sh 'dotnet test'
                }
            }
        }

        stage('Package') {
            steps {
                // Publish the .NET Core project
                script {
                    sh 'dotnet publish -c Release -o ./publish'
                }
            }
        }

        stage('Docker Build') {
            steps {
                // Build Docker image
                script {
                    docker.build("${DOCKER_REGISTRY}/${IMAGE_NAME}:${IMAGE_TAG}", '-f Dockerfile .')
                }
            }
        }

        stage('Docker Push') {
            steps {
                // Push Docker image to registry
                script {
                    docker.withRegistry("https://${DOCKER_REGISTRY}", 'docker-registry-credentials-id') {
                        docker.image("${DOCKER_REGISTRY}/${IMAGE_NAME}:${IMAGE_TAG}").push()
                    }
                }
            }
        }

        stage('Deploy to Docker') {
            steps {
                // Deploy to Docker container
                script {
                    sh "docker stop ${CONTAINER_NAME} || true"
                    sh "docker rm ${CONTAINER_NAME} || true"
                    sh "docker run -d -p 8080:80 --name ${CONTAINER_NAME} ${DOCKER_REGISTRY}/${IMAGE_NAME}:${IMAGE_TAG}"
                }
            }
        }
    }
}
