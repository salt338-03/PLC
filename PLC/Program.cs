//using System;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using Newtonsoft.Json.Linq; // Newtonsoft.Json 사용

//namespace VirtualPLC
//{
//    class VirtualPLC
//    {
//        private Random random;

//        // 슬러리 공급 공정 변수
//        private double slurrySupplyRate;
//        private double slurryVolume;
//        private double slurryTemperature;

//        // 코팅 공정 변수
//        private double coatingSpeed;
//        private double coatingThickness;

//        // 건조 공정 변수
//        private double dryingTemperature;

//        // 초기값 설정
//        public VirtualPLC()
//        {
//            random = new Random();
//            slurrySupplyRate = 12; // 초기값 (12 mL/min)
//            slurryVolume = 100; // 초기 슬러리 용량 (100L)
//            slurryTemperature = random.NextDouble() * 10 + 20; // 20~30°C 랜덤

//            coatingSpeed = random.NextDouble() * 0.7 + 0.8; // 0.8~1.5 m/min 랜덤
//            coatingThickness = random.NextDouble() * 4 + 8; // 8~12 µm 랜덤

//            dryingTemperature = random.NextDouble() * 20 + 80; // 80~100°C 랜덤
//        }

//        public void UpdateProcesses()
//        {
//            // 슬러리 공급 공정 업데이트
//            slurrySupplyRate += (random.NextDouble() * 0.4 - 0.2); // -0.2~0.2
//            slurrySupplyRate = Math.Max(10, Math.Min(15, slurrySupplyRate)); // 10~15 제한
//            slurryVolume -= slurrySupplyRate / 60.0; // 공급 속도에 따른 감소
//            slurryVolume = Math.Max(0, slurryVolume);

//            // 슬러리 온도 업데이트
//            slurryTemperature += (random.NextDouble() * 0.4 - 0.2);
//            slurryTemperature = Math.Max(20, Math.Min(30, slurryTemperature));

//            // 코팅 공정 업데이트
//            coatingSpeed = random.NextDouble() * 0.7 + 0.8; // 0.8~1.5 m/min
//            coatingThickness = random.NextDouble() * 4 + 8; // 8~12 µm

//            // 건조 공정 업데이트
//            dryingTemperature = random.NextDouble() * 20 + 80; // 80~100°C
//        }

//        public void DisplayStatus()
//        {
//            Console.WriteLine("=== 현재 상태 ===");
//            Console.WriteLine($"슬러리 공급 속도: {slurrySupplyRate:F2} mL/min");
//            Console.WriteLine($"슬러리 용량: {slurryVolume:F2} L");
//            Console.WriteLine($"슬러리 온도: {slurryTemperature:F2} °C");
//            Console.WriteLine($"코팅 속도: {coatingSpeed:F2} m/min");
//            Console.WriteLine($"코팅 두께: {coatingThickness:F2} µm");
//            Console.WriteLine($"건조 온도: {dryingTemperature:F2} °C");
//            Console.WriteLine("=====================");
//        }

//        // JSON 데이터 생성 메서드
//        public string GenerateFormattedJsonData()
//        {
//            var timestamp = DateTime.Now.ToString("o"); // ISO 8601 형식 시간

//            var root = new JObject
//            {
//                ["SlurryTank"] = new JObject
//                {
//                    ["Timestamp"] = timestamp,
//                    ["SupplySpeed"] = slurrySupplyRate,
//                    ["RemainingVolume"] = slurryVolume,
//                    ["Temperature"] = slurryTemperature
//                },
//                ["CoatingProcess"] = new JObject
//                {
//                    ["Timestamp"] = timestamp,
//                    ["Speed"] = coatingSpeed,
//                    ["Thickness"] = coatingThickness
//                },
//                ["DryingProcess"] = new JObject
//                {
//                    ["Timestamp"] = timestamp,
//                    ["Temperature"] = dryingTemperature
//                }
//            };

//            return root.ToString(); // JSON 문자열 반환
//        }

//        // TCP 클라이언트를 사용해 JSON 데이터 전송
//        public void SendDataToServer(string serverIp, int port)
//        {
//            string jsonData = GenerateFormattedJsonData();
//            Console.WriteLine("Generated JSON Data:");
//            Console.WriteLine(jsonData); // 콘솔에 JSON 데이터 출력

//            try
//            {
//                using (var client = new TcpClient(serverIp, port))
//                using (var stream = client.GetStream())
//                {
//                    byte[] data = Encoding.UTF8.GetBytes(jsonData + "\n");
//                    stream.Write(data, 0, data.Length);
//                    Console.WriteLine($"Sent JSON Data to server: {serverIp}:{port}");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine($"Failed to send data to server: {ex.Message}");
//            }
//        }

//        static void Main(string[] args)
//        {
//            var plc = new VirtualPLC();

//            // 서버 IP와 포트 설정
//            string serverIp1 = "192.168.1.173"; // 창헌
//            string serverIp2 = "192.168.1.196"; // 유석
//            string serverIp3 = "127.0.0.1"; // 아현
//            int serverPort = 8080; // WPF 서버 포트

//            // 상태 업데이트와 데이터 전송 반복 실행
//            while (true)
//            {
//                var startTime = DateTime.Now;

//                plc.UpdateProcesses(); // 공정 상태 업데이트
//                plc.DisplayStatus(); // 현재 상태 출력
//                //plc.SendDataToServer(serverIp1, serverPort); // 서버로 JSON 데이터 전송
//                //plc.SendDataToServer(serverIp2, serverPort);
//                plc.SendDataToServer(serverIp3, serverPort);

//                // 경과 시간 계산
//                var elapsedTime = (DateTime.Now - startTime).TotalMilliseconds;

//                // 남은 시간 계산
//                var sleepTime = 1000 - elapsedTime;

//                if (sleepTime > 0)
//                {
//                    Thread.Sleep((int)sleepTime); // 남은 시간만큼 대기
//                }
//                else
//                {
//                    // 처리 시간이 1초를 초과한 경우 경고 메시지 출력
//                    Console.WriteLine("Warning: Processing took longer than 1 second.");
//                }
//            }
//        }
//    }
//}

using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace VirtualPLC
{
    class VirtualPLC
    {
        private Random random;

        // 슬러리 공급 공정 변수
        private double slurrySupplyRate;
        private double slurryVolume;
        private double slurryTemperature;

        // 코팅 공정 변수
        private double coatingSpeed;
        private double coatingThickness;

        // 건조 공정 변수
        private double dryingTemperature;

        private TcpClient client;
        private NetworkStream stream;
        private bool isRunning;

        public VirtualPLC()
        {
            random = new Random();
            slurrySupplyRate = 12; // 초기값 (12 mL/min)
            slurryVolume = 55; // 초기 슬러리 용량 (100L)
            slurryTemperature = random.NextDouble() * 10 + 20; // 20~30°C 랜덤

            coatingSpeed = random.NextDouble() * 0.7 + 0.8; // 0.8~1.5 m/min 랜덤
            coatingThickness = random.NextDouble() * 4 + 8; // 8~12 µm 랜덤

            dryingTemperature = random.NextDouble() * 20 + 80; // 80~100°C 랜덤
        }

        public void ConnectToServer(string serverIp, int port)
        {
            try
            {
                client = new TcpClient(serverIp, port);
                stream = client.GetStream();
                isRunning = true;
                Console.WriteLine($"Connected to server: {serverIp}:{port}");
            }
            catch (Exception ex)
            {
                LogError($"Failed to connect: {ex.Message}");
                Console.WriteLine("Retrying connection in 5 seconds...");
                Thread.Sleep(5000);
                ConnectToServer(serverIp, port);
            }
        }

        public void UpdateProcesses()
        {
            // 슬러리 공급 공정 업데이트
            slurrySupplyRate += (random.NextDouble() * 0.4 - 0.2); // -0.2~0.2
            slurrySupplyRate = Math.Max(10, Math.Min(15, slurrySupplyRate)); // 10~15 제한
            slurryVolume -= slurrySupplyRate / 60.0; // 공급 속도에 따른 감소
            slurryVolume = Math.Max(0, slurryVolume);

            // 슬러리 온도 업데이트
            slurryTemperature += (random.NextDouble() * 0.4 - 0.2);
            slurryTemperature = Math.Max(20, Math.Min(30, slurryTemperature));

            // 코팅 공정 업데이트
            coatingSpeed = random.NextDouble() * 0.7 + 0.8; // 0.8~1.5 m/min
            coatingThickness = random.NextDouble() * 4 + 8; // 8~12 µm

            // 건조 공정 업데이트
            dryingTemperature = random.NextDouble() * 20 + 80; // 80~100°C
        }

        public void DisplayStatus()
        {
            //Console.WriteLine("=== 현재 상태 ===");
            //Console.WriteLine($"슬러리 공급 속도: {slurrySupplyRate:F2} mL/min");
            //Console.WriteLine($"슬러리 용량: {slurryVolume:F2} L");
            //Console.WriteLine($"슬러리 온도: {slurryTemperature:F2} °C");
            //Console.WriteLine($"코팅 속도: {coatingSpeed:F2} m/min");
            //Console.WriteLine($"코팅 두께: {coatingThickness:F2} µm");
            //Console.WriteLine($"건조 온도: {dryingTemperature:F2} °C");
            //Console.WriteLine("=====================");
        }

        public string GenerateFormattedJsonData()
        {
            var timestamp = DateTime.Now.ToString("o"); // ISO 8601 형식 시간

            var root = new JObject
            {
                ["SlurryTank"] = new JObject
                {
                    ["Timestamp"] = timestamp,
                    ["SupplySpeed"] = slurrySupplyRate,
                    ["RemainingVolume"] = slurryVolume,
                    ["Temperature"] = slurryTemperature
                },
                ["CoatingProcess"] = new JObject
                {
                    ["Timestamp"] = timestamp,
                    ["Speed"] = coatingSpeed,
                    ["Thickness"] = coatingThickness
                },
                ["DryingProcess"] = new JObject
                {
                    ["Timestamp"] = timestamp,
                    ["Temperature"] = dryingTemperature
                }
            };
            Console.WriteLine(root);
            return root.ToString(); // JSON 문자열 반환
           
        }

        public void SendData()
        {
            try
            {
                while (isRunning && client != null && client.Connected)
                {
                    UpdateProcesses();
                    DisplayStatus();

                    string jsonData = GenerateFormattedJsonData();
                    byte[] jsonDataBytes = Encoding.UTF8.GetBytes(jsonData);

                    // 메시지 길이 (4바이트)와 데이터 결합
                    byte[] lengthPrefix = BitConverter.GetBytes(jsonDataBytes.Length);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(lengthPrefix);
                    }

                    // 길이 정보와 실제 데이터를 전송
                    stream.Write(lengthPrefix, 0, lengthPrefix.Length);
                    stream.Write(jsonDataBytes, 0, jsonDataBytes.Length);

                    Console.WriteLine("Sent JSON Data to server with length prefix.");

                    Thread.Sleep(1000); // 1초 대기
                }
            }
            catch (Exception ex)
            {
                LogError($"Error during data transmission: {ex.Message}");
                isRunning = false;
                ConnectToServer("127.0.0.1", 8080); // 연결 재시도
            }
        }

        private void LogError(string message)
        {
            File.AppendAllText("error.log", $"{DateTime.Now}: {message}\n");
        }

        static void Main(string[] args)
        {
            var plc = new VirtualPLC();

            // 서버 IP와 포트 설정
            string serverIp = "127.0.0.1"; // 서버 IP
            int serverPort = 8080; // 서버 포트

            plc.ConnectToServer(serverIp, serverPort);

            Thread sendDataThread = new Thread(plc.SendData);
            sendDataThread.Start();
        }
    }
}
