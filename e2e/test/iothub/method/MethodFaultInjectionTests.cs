﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices.E2ETests.Helpers;
using Microsoft.Azure.Devices.E2ETests.Helpers.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.Devices.E2ETests.Methods
{
    [TestClass]
    [TestCategory("E2E")]
    [TestCategory("IoTHub")]
    [TestCategory("FaultInjection")]
    public class MethodFaultInjectionTests : E2EMsTestBase
    {
        private readonly string DevicePrefix = $"E2E_{nameof(MethodFaultInjectionTests)}_";
        private const string DeviceResponseJson = "{\"name\":\"e2e_test\"}";
        private const string ServiceRequestJson = "{\"a\":123}";
        private const string MethodName = "MethodE2ETest";

        [LoggedTestMethod]
        public async Task Method_DeviceReceivesMethodAndResponseRecovery_MqttWs()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Mqtt_WebSocket_Only,
                    FaultInjection.FaultType_Tcp,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodGracefulShutdownRecovery_Mqtt()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Mqtt_Tcp_Only,
                    FaultInjection.FaultType_GracefulShutdownMqtt,
                    FaultInjection.FaultCloseReason_Bye,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceReceivesMethodAndResponseRecovery_Mqtt()
        {
            await SendMethodAndRespondRecoveryAsync(Client.TransportType.Mqtt_Tcp_Only,
                    FaultInjection.FaultType_Tcp,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodGracefulShutdownRecovery_MqttWs()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Mqtt_WebSocket_Only,
                    FaultInjection.FaultType_GracefulShutdownMqtt,
                    FaultInjection.FaultCloseReason_Bye,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodTcpConnRecovery_Amqp()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Amqp_Tcp_Only,
                    FaultInjection.FaultType_Tcp,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodTcpConnRecovery_AmqpWs()
        {
            await SendMethodAndRespondRecoveryAsync(Client.TransportType.Amqp_WebSocket_Only,
                    FaultInjection.FaultType_Tcp,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodAmqpConnLostRecovery_Amqp()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Amqp_Tcp_Only,
                    FaultInjection.FaultType_AmqpConn,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodAmqpConnLostRecovery_AmqpWs()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Amqp_WebSocket_Only,
                    FaultInjection.FaultType_AmqpConn,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodSessionLostRecovery_Amqp()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Amqp_Tcp_Only,
                    FaultInjection.FaultType_AmqpSess,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodSessionLostRecovery_AmqpWs()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Amqp_WebSocket_Only,
                    FaultInjection.FaultType_AmqpSess,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodReqLinkDropRecovery_Amqp()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Amqp_Tcp_Only,
                    FaultInjection.FaultType_AmqpMethodReq,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodReqLinkDropRecovery_AmqpWs()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Amqp_WebSocket_Only,
                    FaultInjection.FaultType_AmqpMethodReq,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodRespLinkDropRecovery_Amqp()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Amqp_Tcp_Only,
                    FaultInjection.FaultType_AmqpMethodResp,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodRespLinkDropRecovery_AmqpWs()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Amqp_WebSocket_Only,
                    FaultInjection.FaultType_AmqpMethodResp,
                    FaultInjection.FaultCloseReason_Boom,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodGracefulShutdownRecovery_Amqp()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Amqp_Tcp_Only,
                    FaultInjection.FaultType_GracefulShutdownAmqp,
                    FaultInjection.FaultCloseReason_Bye,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        [LoggedTestMethod]
        public async Task Method_DeviceMethodGracefulShutdownRecovery_AmqpWs()
        {
            await SendMethodAndRespondRecoveryAsync(
                    Client.TransportType.Amqp_WebSocket_Only,
                    FaultInjection.FaultType_GracefulShutdownAmqp,
                    FaultInjection.FaultCloseReason_Bye,
                    FaultInjection.DefaultDelayInSec)
                .ConfigureAwait(false);
        }

        private async Task ServiceSendMethodAndVerifyResponseAsync(string deviceName, string methodName, string respJson, string reqJson)
        {
            var sw = new Stopwatch();
            sw.Start();
            bool done = false;
            ExceptionDispatchInfo exceptionDispatchInfo = null;
            int attempt = 0;

            while (!done && sw.ElapsedMilliseconds < FaultInjection.RecoveryTimeMilliseconds)
            {
                attempt++;
                try
                {
                    using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(Configuration.IoTHub.ConnectionString);

                    Logger.Trace($"{nameof(ServiceSendMethodAndVerifyResponseAsync)}: Invoke method {methodName}.");
                    CloudToDeviceMethodResult response =
                        await serviceClient
                            .InvokeDeviceMethodAsync(
                                deviceName,
                                new CloudToDeviceMethod(methodName, TimeSpan.FromMinutes(5)).SetPayloadJson(reqJson))
                            .ConfigureAwait(false);

                    Logger.Trace($"{nameof(ServiceSendMethodAndVerifyResponseAsync)}: Method status: {response.Status}.");

                    Assert.AreEqual(200, response.Status, $"The expected respose status should be 200 but was {response.Status}");
                    string payload = response.GetPayloadAsJson();
                    Assert.AreEqual(respJson, payload, $"The expected respose payload should be {respJson} but was {payload}");

                    await serviceClient.CloseAsync().ConfigureAwait(false);
                    done = true;
                }
                catch (DeviceNotFoundException ex)
                {
                    exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex);
                    Logger.Trace($"{nameof(ServiceSendMethodAndVerifyResponseAsync)}: [Tried {attempt} time(s)] ServiceClient exception caught: {ex}.");
                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }

            if (!done && (exceptionDispatchInfo != null))
            {
                exceptionDispatchInfo.Throw();
            }
        }

        private async Task SendMethodAndRespondRecoveryAsync(Client.TransportType transport, string faultType, string reason, int delayInSec)
        {
            TestDeviceCallbackHandler testDeviceCallbackHandler = null;
            using var cts = new CancellationTokenSource(FaultInjection.RecoveryTimeMilliseconds);

            // Configure the callback and start accepting method calls.
            Func<DeviceClient, TestDevice, Task> initOperation = async (deviceClient, testDevice) =>
            {
                testDeviceCallbackHandler = new TestDeviceCallbackHandler(deviceClient, Logger);
                await testDeviceCallbackHandler
                    .SetDeviceReceiveMethodAsync(MethodName, DeviceResponseJson, ServiceRequestJson)
                    .ConfigureAwait(false);
            };

            // Call the method from the service side and verify the device received the call.
            Func<DeviceClient, TestDevice, Task> testOperation = async (deviceClient, testDevice) =>
            {
                Task serviceSendTask = ServiceSendMethodAndVerifyResponseAsync(testDevice.Id, MethodName, DeviceResponseJson, ServiceRequestJson);
                Task methodReceivedTask = testDeviceCallbackHandler.WaitForMethodCallbackAsync(cts.Token);

                var tasks = new List<Task>() { serviceSendTask, methodReceivedTask };
                while (tasks.Count > 0)
                {
                    Task completedTask = await Task.WhenAny(tasks).ConfigureAwait(false);
                    completedTask.GetAwaiter().GetResult();
                    tasks.Remove(completedTask);
                }
            };

            await FaultInjection
                .TestErrorInjectionAsync(
                    DevicePrefix,
                    TestDeviceType.Sasl,
                    transport,
                    faultType,
                    reason,
                    delayInSec,
                    FaultInjection.DefaultDelayInSec,
                    initOperation,
                    testOperation,
                    () => { return Task.FromResult<bool>(false); },
                    Logger)
                .ConfigureAwait(false);
        }
    }
}
