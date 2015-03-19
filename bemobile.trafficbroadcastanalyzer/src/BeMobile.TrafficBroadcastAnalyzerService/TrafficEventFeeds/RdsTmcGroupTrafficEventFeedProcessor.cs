using BeMobile.TrafficBroadcastAnalyzerService.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using BeMobile.Rds;
using BeMobile.Rds.Oda;
using BeMobile.Rds.Tmc;
using BeMobile.TrafficBroadcastAnalyzerService.RdsDecodeingAndStream;
using BeMobile.TrafficBroadcastAnalyzerService.TrafficEvents;
using Timer = System.Timers.Timer;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds
{
    internal sealed class RdsTmcGroupTrafficEventFeedProcessor : IDisposable
    {
        private const int ProcessingTimerInterval = 100000;
        private readonly RdsTmcConfigurationEntry configurationEntry;
        private readonly Timer processingTimer;
        private bool isProcessing;
        private bool isDisposed;
        private Thread DecodingThread = new Thread( new ThreadStart(DecoderTest.StartDecoding));
        private MultiThreadSocketRdmListener _socketRead = new MultiThreadSocketRdmListener();
        private Thread SocketInRead;

        internal RdsTmcGroupTrafficEventFeedProcessor(RdsTmcConfigurationEntry configurationEntry)
        {
            this.processingTimer = new Timer();
            this.processingTimer.Interval = ProcessingTimerInterval;
            this.processingTimer.Elapsed += this.OnProcessingTimerElapsed;
            this.configurationEntry = configurationEntry;
            this.TrafficEventFeed = new RdsTmcGroupTrafficEventFeed();
            this.TrafficEventFeed.Metadata = new RdsTmcGroupTrafficEventFeedMetadata();
            this.TrafficEventFeed.TrafficEvents = new ObservableCollection<TmcTrafficEvent>();

           
        }

        internal RdsTmcGroupTrafficEventFeed TrafficEventFeed { get; private set; }

        public void Start()
        {
            if (SocketInRead == null)
            {
                SocketInRead = new Thread(new ThreadStart(_socketRead.ConnectListen));
                SocketInRead.Start();
            }

            if (DecodingThread.IsAlive != true)
            {
              DecodingThread.Start();
            }

            if (this.processingTimer != null)
            {
                this.processingTimer.Start();
            }
        }

        public void Stop()
        {
            if (this.processingTimer != null)
            {
                this.processingTimer.Stop();
            }
            if(SocketInRead != null)
            {
                _socketRead.closeCon();
                SocketInRead.Abort();
            }
            DecodingThread.Abort();
        }

        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }
            if (this.processingTimer != null)
            {
                this.processingTimer.Dispose();
            }
            this.isDisposed = true;
        }

        private void OnProcessingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Prevent the timer from caling the same instance multiple times at the same time.
            if (this.isProcessing)
            {
                return;
            }
            this.isProcessing = true;
            if (this.TrafficEventFeed.TrafficEvents.Count > 180)
            {
                this.TrafficEventFeed.TrafficEvents.Clear();
            }
            this.ProcessTrafficEvents();

            TrafficBroadcastAnalyzerService.isCompatring = true;
            TrafficBroadcastAnalyzerService.Service.CompareXmlAndRds();
            TrafficBroadcastAnalyzerService.isCompatring = false;
           this.isProcessing = false;
        }

        private void ProcessTrafficEvents()
        {
            // TODO: process RDS TMC group traffic events.
            int lenghtofque = DecoderTest.DecodedGroupfeed.Count;
            for (int i=0; i < lenghtofque; i++)
            {
                RdsGroup temp;
                if (DecoderTest.DecodedGroupfeed.TryDequeue(out temp))
                {
                    if (temp.Metadata.Exception == null)
                    {
                        var b = checkGroup(temp);
                        
                    }
                        
                   
                }

            }

        }

        private TmcTrafficEvent checkGroup(RdsGroup data)
        {
            TmcTrafficEvent temp = new TmcTrafficEvent();

            var datafields = data.DataFields as Group8ATmcEventSingleGroupDataFields;
            if (datafields != null)
            {
               
                if (datafields.CodingDirection.HasValue)
                {

                    if (datafields.CodingDirection.Value == 1)
                    {
                        temp.CodingDirection = TmcCodingDirection.Positive;
                    }
                    else
                    {
                        temp.CodingDirection = TmcCodingDirection.Negative;
                    }

                }
                if(datafields.Location.HasValue)
                temp.LocationCode = datafields.Location.Value;

                TmcEventCodeHistoryEntry tempEvent = new TmcEventCodeHistoryEntry();

                if (datafields.Event.HasValue)
                    tempEvent.EventCode = datafields.Event.Value;

                if (datafields.Extent.HasValue)
                    tempEvent.Extent = datafields.Extent.Value;

                temp.EventCodeHistory = new List<TmcEventCodeHistoryEntry>();
                temp.EventCodeHistory.Add(tempEvent);

                temp.Source = data;

               
                this.TrafficEventFeed.TrafficEvents.Add(temp);

            }

       
         

          
           

            var datafieldmultisec = data.DataFields as Group8ATmcEventMultiGroupSubseqDataFields;
            if(datafieldmultisec != null)
            {
                if(datafieldmultisec.MultiGroupTmcEvent  != null)
                if (datafieldmultisec.MultiGroupTmcEvent.Metadata.IsDecoded == true)
                {
                

                    TmcTrafficEvent tempSub = new TmcTrafficEvent();

                    if (datafieldmultisec.MultiGroupTmcEvent.DataFields.CodingDirection.HasValue)
                    {
                        if (datafieldmultisec.MultiGroupTmcEvent.DataFields.CodingDirection.Value == 1)
                        {
                            tempSub.CodingDirection = TmcCodingDirection.Positive;
                        }
                        else
                        {
                            tempSub.CodingDirection = TmcCodingDirection.Negative;
                            
                        }


                    }

                    

                      

                    foreach (var groups in datafieldmultisec.MultiGroupTmcEvent.Groups)
                    {
                       

                        var tempsubgroup = groups.DataFields as Group8ATmcEventMultiGroupSubseqDataFields;
                        if (tempsubgroup != null)
                        {
                                tempSub = new TmcTrafficEvent();

                          if (tempsubgroup.MultiGroupTmcEvent.DataFields.CodingDirection.HasValue)
                            {
                                 if (tempsubgroup.MultiGroupTmcEvent.DataFields.CodingDirection.Value == 1)
                                 {
                                   tempSub.CodingDirection = TmcCodingDirection.Positive;
                                 }
                                else
                                {
                                   tempSub.CodingDirection = TmcCodingDirection.Negative;
                            
                                 }


                    }

                          if (tempsubgroup.MultiGroupTmcEvent.DataFields.Location.HasValue)
                          {
                              tempSub.LocationCode = tempsubgroup.MultiGroupTmcEvent.DataFields.Location.Value;
                          }
                            TmcEventCodeHistoryEntry historyEntry = new TmcEventCodeHistoryEntry();
                            if (tempsubgroup.MultiGroupTmcEvent.DataFields.Event.HasValue)
                            {
                                historyEntry.EventCode = tempsubgroup.MultiGroupTmcEvent.DataFields.Event.Value;
                            }

                            if (tempsubgroup.MultiGroupTmcEvent.DataFields.Extent.HasValue)
                            {
                                historyEntry.Extent = tempsubgroup.MultiGroupTmcEvent.DataFields.Extent.Value;
                            }

                            tempSub.EventCodeHistory = new List<TmcEventCodeHistoryEntry>();
                            tempSub.EventCodeHistory.Add(historyEntry);
                            tempSub.Source = data;
                                this.TrafficEventFeed.TrafficEvents.Add(tempSub);
                        }
                    }
                }
                    

            }

            //dit is de meta data van de stream
            var datafieldHeader3A = data.DataFields as Group3AOda8ATmcDataFields;
            {
                if (datafieldHeader3A != null)
                {
                    

                   var datafieldHeader3AVar0 = datafieldHeader3A as Group3AOda8ATmcVar0DataFields;
                    if (datafieldHeader3AVar0 != null)
                    {
                        TrafficEventFeed.Metadata.LocationTableNumber = datafieldHeader3AVar0.LocationTableNumber;
                        TrafficEventFeed.Metadata.National = datafieldHeader3AVar0.National;
                        TrafficEventFeed.Metadata.Regional = datafieldHeader3AVar0.Regional;
                        TrafficEventFeed.Metadata.Urban = datafieldHeader3AVar0.Urban;
                        TrafficEventFeed.Metadata.International = datafieldHeader3AVar0.International;
                    }

                    var dataFieldHeader3AVar1 = datafieldHeader3A as Group3AOda8ATmcVar1DataFields;
                    if (dataFieldHeader3AVar1 != null)
                    {
                        TrafficEventFeed.Metadata.LocationTableCountryCode =
                            dataFieldHeader3AVar1.LocationTableCountryCode;
                   }

                    var datafieldHeader3AVar2 = datafieldHeader3A as Group3AOda8ATmcVar2DataFields;
                    if (datafieldHeader3AVar2 != null)
                    {
                        TrafficEventFeed.Metadata.LocationTableExtendedCountryCode =
                            datafieldHeader3AVar2.LocationTableExtendedCountryCode;
                        
                    }

                    //Console.WriteLine(TrafficEventFeed.Metadata.ToString());

                }
            }
            return null;
        }
    }
}
