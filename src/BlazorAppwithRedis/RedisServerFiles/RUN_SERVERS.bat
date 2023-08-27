
if not exist "Redis-x64-3.0.504_master" (
  mkdir "Redis-x64-3.0.504_master"
  tar -xf Redis-x64-3.0.504.zip -C "Redis-x64-3.0.504_master"
)

if not exist "Redis-x64-3.0.504_slave" (
  mkdir "Redis-x64-3.0.504_slave"
  tar -xf Redis-x64-3.0.504.zip -C "Redis-x64-3.0.504_slave"
)

start "Start RedisMaster" Redis-x64-3.0.504_master\redis-server.exe redis.windows.master.conf
start "Start RedisSlave" Redis-x64-3.0.504_slave\redis-server.exe redis.windows.slave.conf
