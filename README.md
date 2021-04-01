# Demo Kafka in docker with .NET5 Pub/Sub projects

1. Make sure docker machine is running
2. Go to command prompt and run this command: $ docker-compose up -d
3. Once step #2 is done, run this command to make sure Kafka is running: $ docker-compose ps
4. Now run the publisher with correct BootstrapServers and topic info
5. Run the subscriber (with correct BootstrapServers and topic ) to read messages from Kafka