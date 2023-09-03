// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/Chat.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace backend.Protos {
  public static partial class RealTimeChatService
  {
    static readonly string __ServiceName = "RealTimeChatService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::backend.Protos.MessageRequest> __Marshaller_MessageRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::backend.Protos.MessageRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::backend.Protos.MessageResponse> __Marshaller_MessageResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::backend.Protos.MessageResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::backend.Protos.UserID> __Marshaller_UserID = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::backend.Protos.UserID.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::backend.Protos.UsersIDsListResponse> __Marshaller_UsersIDsListResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::backend.Protos.UsersIDsListResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::backend.Protos.MessageRequest, global::backend.Protos.MessageResponse> __Method_SendMessage = new grpc::Method<global::backend.Protos.MessageRequest, global::backend.Protos.MessageResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SendMessage",
        __Marshaller_MessageRequest,
        __Marshaller_MessageResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::backend.Protos.UserID, global::backend.Protos.UsersIDsListResponse> __Method_GetUserFollowingFollowers = new grpc::Method<global::backend.Protos.UserID, global::backend.Protos.UsersIDsListResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetUserFollowingFollowers",
        __Marshaller_UserID,
        __Marshaller_UsersIDsListResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::backend.Protos.ChatReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of RealTimeChatService</summary>
    [grpc::BindServiceMethod(typeof(RealTimeChatService), "BindService")]
    public abstract partial class RealTimeChatServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::backend.Protos.MessageResponse> SendMessage(global::backend.Protos.MessageRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::backend.Protos.UsersIDsListResponse> GetUserFollowingFollowers(global::backend.Protos.UserID request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(RealTimeChatServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SendMessage, serviceImpl.SendMessage)
          .AddMethod(__Method_GetUserFollowingFollowers, serviceImpl.GetUserFollowingFollowers).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, RealTimeChatServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_SendMessage, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::backend.Protos.MessageRequest, global::backend.Protos.MessageResponse>(serviceImpl.SendMessage));
      serviceBinder.AddMethod(__Method_GetUserFollowingFollowers, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::backend.Protos.UserID, global::backend.Protos.UsersIDsListResponse>(serviceImpl.GetUserFollowingFollowers));
    }

  }
}
#endregion