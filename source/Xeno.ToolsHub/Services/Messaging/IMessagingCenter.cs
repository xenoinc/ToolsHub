/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-1-2
 * Author:  Damian Suess
 * File:    IMessagingCenter.cs
 * Description:
 *MessagingCenter interface
 */

namespace Xeno.ToolsHub.Services.Messaging
{
  using System;

  public interface IMessagingCenter
  {
    void Send<TSender, TArgs>(TSender sender, string message, TArgs args)
      where TSender : class;

    void Send<TSender>(TSender sender, string message)
      where TSender : class;

    void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback, TSender source = null)
      where TSender : class;

    void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback, TSender source = null)
      where TSender : class;

    void Unsubscribe<TSender, TArgs>(object subscriber, string message)
      where TSender : class;

    void Unsubscribe<TSender>(object subscriber, string message)
      where TSender : class;
  }
}
