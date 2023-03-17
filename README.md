# 并行异步demo

## 开发环境
1. VS2022
2. .NET 6

## CSharp_Java_Go分支
使用C#写了一段代码，请用Java和Go实现，以进行对比

## 说明
1. Server端需要处理143000个请求
2. MiddleServer端需要处理1000个请求，其中500个GetValues接口请求，500个Query2接口
3. MiddleServer端需要请求143000次Server端的接口
4. Client端需要请求1000次MiddleServer端的接口，其中500次GetValues接口请求，500次Query2接口请求
5. Client端请求MiddleServer端的GetValues接口和Query2接口，并发量分别为200

## 性能
笔记本CPU：I5-8265U，功耗：32W
1. Server端处理完143000个请求，耗时大约16秒
2. MiddleServer端处理完1000个请求，耗时大约16秒
3. Client端请求1000次Server端接口，耗时大约16秒

## 对比什么？
1. 性能(并不专业，所以粗略对比一下，只要没有数量级的差距就没问题)
2. 代码简捷程度(代码是否容易编写，容易阅读，容易维护)



